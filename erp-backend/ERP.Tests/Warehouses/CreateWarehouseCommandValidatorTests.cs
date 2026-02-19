using ERP.Application.Features.Warehouses.Commands;
using ERP.Domain.Enums;
using FluentAssertions;

namespace ERP.Tests.Warehouses;

public class CreateWarehouseCommandValidatorTests
{
    private readonly CreateWarehouseCommandValidator _validator = new();

    private static CreateWarehouseCommand ValidMain() =>
        new()
        {
            Name = "Main Warehouse",
            BranchType = BranchType.Main
        };

    private static CreateWarehouseCommand ValidBranch() =>
        new()
        {
            Name = "Branch Warehouse",
            BranchType = BranchType.Branch,
            ParentWarehouseId = "parent-1"
        };

    // ─── Valid cases ──────────────────────────────────────────────────────────

    [Fact]
    public void Validate_IsValid_ForMainWarehouseWithoutParent()
    {
        var result = _validator.Validate(ValidMain());

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_IsValid_ForBranchWarehouseWithParent()
    {
        var result = _validator.Validate(ValidBranch());

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_IsValid_ForSubWarehouseWithParent()
    {
        var command = new CreateWarehouseCommand
        {
            Name = "Sub Warehouse",
            BranchType = BranchType.Sub,
            ParentWarehouseId = "branch-1"
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_IsValid_WithAllOptionalFieldsProvided()
    {
        var command = ValidMain();
        command.City = "Bangkok";
        command.Location = "Zone A";
        command.Address = "123 Main Road";
        command.Country = "Thailand";
        command.ContactPerson = "John Doe";
        command.Phone = "0812345678";

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    // ─── Name validation ──────────────────────────────────────────────────────

    [Fact]
    public void Validate_Fails_WhenNameIsEmpty()
    {
        var command = ValidMain();
        command.Name = "";

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name" &&
                                            e.ErrorMessage == "Warehouse name is required");
    }

    [Fact]
    public void Validate_Fails_WhenNameExceeds50Characters()
    {
        var command = ValidMain();
        command.Name = new string('A', 51);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name" &&
                                            e.ErrorMessage == "Warehouse name cannot exceed 50 characters");
    }

    [Fact]
    public void Validate_IsValid_WhenNameIsExactly50Characters()
    {
        var command = ValidMain();
        command.Name = new string('A', 50);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    // ─── BranchType ───────────────────────────────────────────────────────────

    [Fact]
    public void Validate_Fails_WhenBranchTypeIsInvalid()
    {
        var command = new CreateWarehouseCommand
        {
            Name = "Test WH",
            BranchType = (BranchType)99
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "BranchType");
    }

    // ─── ParentWarehouseId rules ──────────────────────────────────────────────

    [Fact]
    public void Validate_Fails_WhenBranchWarehouseHasNoParent()
    {
        var command = new CreateWarehouseCommand
        {
            Name = "Branch WH",
            BranchType = BranchType.Branch
            // ParentWarehouseId intentionally omitted
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ParentWarehouseId" &&
                                            e.ErrorMessage == "Parent warehouse is required for Branch and Sub warehouses");
    }

    [Fact]
    public void Validate_Fails_WhenSubWarehouseHasNoParent()
    {
        var command = new CreateWarehouseCommand
        {
            Name = "Sub WH",
            BranchType = BranchType.Sub
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ParentWarehouseId" &&
                                            e.ErrorMessage == "Parent warehouse is required for Branch and Sub warehouses");
    }

    [Fact]
    public void Validate_Fails_WhenMainWarehouseHasParent()
    {
        var command = new CreateWarehouseCommand
        {
            Name = "Main WH",
            BranchType = BranchType.Main,
            ParentWarehouseId = "some-parent"
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ParentWarehouseId" &&
                                            e.ErrorMessage == "Main warehouse cannot have a parent warehouse");
    }

    // ─── Optional field length limits ─────────────────────────────────────────

    [Theory]
    [InlineData(51, "City cannot exceed 50 characters", "City")]
    public void Validate_Fails_WhenCityExceedsMaxLength(int length, string expectedMessage, string propertyName)
    {
        var command = ValidMain();
        command.City = new string('X', length);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == propertyName && e.ErrorMessage == expectedMessage);
    }

    [Theory]
    [InlineData(101, "Location cannot exceed 100 characters", "Location")]
    [InlineData(256, "Address cannot exceed 255 characters", "Address")]
    public void Validate_Fails_WhenOptionalStringFieldExceedsLimit(int length, string expectedMessage, string propertyName)
    {
        var command = ValidMain();

        if (propertyName == "Location") command.Location = new string('X', length);
        else if (propertyName == "Address") command.Address = new string('X', length);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == propertyName && e.ErrorMessage == expectedMessage);
    }

    [Fact]
    public void Validate_Fails_WhenCountryExceeds50Characters()
    {
        var command = ValidMain();
        command.Country = new string('X', 51);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Country");
    }

    [Fact]
    public void Validate_Fails_WhenContactPersonExceeds100Characters()
    {
        var command = ValidMain();
        command.ContactPerson = new string('X', 101);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ContactPerson");
    }

    [Fact]
    public void Validate_Fails_WhenPhoneExceeds20Characters()
    {
        var command = ValidMain();
        command.Phone = new string('0', 21);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Phone");
    }
}
