using ERP.Application.Features.ProductUnitConversion.Commands;
using FluentAssertions;

namespace ERP.Tests.ProductUnitConversion.Validators;

public class UpdateProductUnitConversionCommandValidatorTests
{
    private readonly UpdateProductUnitConversionCommandValidator _validator = new();

    private static UpdateProductUnitConversionCommand ValidCommand() =>
        new()
        {
            Id = "1",
            ProductId = "P1",
            FromUnitId = "U1",
            ToUnitId = "U2",
            Factor = 100
        };

    [Fact]
    public void Validate_IsValid_WhenDataIsCorrect()
    {
        var result = _validator.Validate(ValidCommand());
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_Fails_WhenIdIsEmpty()
    {
        var command = ValidCommand();
        command.Id = "";

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id");
    }

    [Fact]
    public void Validate_Fails_WhenProductIdIsEmpty()
    {
        var command = ValidCommand();
        command.ProductId = "";

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ProductId");
    }

    [Fact]
    public void Validate_Fails_WhenFromUnitIdIsEmpty()
    {
        var command = ValidCommand();
        command.FromUnitId = "";

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FromUnitId");
    }

    [Fact]
    public void Validate_Fails_WhenToUnitIdIsEmpty()
    {
        var command = ValidCommand();
        command.ToUnitId = "";

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ToUnitId");
    }

    [Fact]
    public void Validate_Fails_WhenToUnitIdEqualsFromUnitId()
    {
        var command = ValidCommand();
        command.FromUnitId = "U1";
        command.ToUnitId = "U1";

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ToUnitId");
    }

    [Fact]
    public void Validate_Fails_WhenFactorIsZero()
    {
        var command = ValidCommand();
        command.Factor = 0;

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Factor");
    }

    [Fact]
    public void Validate_Fails_WhenFactorIsNegative()
    {
        var command = ValidCommand();
        command.Factor = -5;

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Factor");
    }
}
