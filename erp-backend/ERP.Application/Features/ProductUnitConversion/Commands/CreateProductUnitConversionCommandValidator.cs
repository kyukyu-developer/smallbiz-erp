using FluentValidation;

namespace ERP.Application.Features.ProductUnitConversion.Commands
{
    public class CreateProductUnitConversionCommandValidator : AbstractValidator<CreateProductUnitConversionCommand>
    {
        public CreateProductUnitConversionCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required");

            RuleFor(x => x.FromUnitId)
                .NotEmpty().WithMessage("From Unit ID is required");

            RuleFor(x => x.ToUnitId)
                .NotEmpty().WithMessage("To Unit ID is required")
                .NotEqual(x => x.FromUnitId).WithMessage("To Unit must be different from From Unit");

            RuleFor(x => x.Factor)
                .GreaterThan(0).WithMessage("Factor must be greater than 0");
        }
    }
}
