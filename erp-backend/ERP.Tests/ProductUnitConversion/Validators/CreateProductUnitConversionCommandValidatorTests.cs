using ERP.Application.Features.ProductUnitConversion.Commands;
using FluentAssertions;

namespace ERP.Tests.ProductUnitConversion.Validators;

public class CreateProductUnitConversionCommandValidatorTests
{
    private readonly CreateProductUnitConversionCommandValidator _validator = new();

    private static CreateProductUnitConversionCommand ValidCommand() =>
        new()
        {
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
        command.Factor = -1;

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Factor");
    }

    [Fact]
    public void Validate_IsValid_WhenFactorIsGreaterThanZero()
    {
        var command = ValidCommand();
        command.Factor = 0.01m;

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}
