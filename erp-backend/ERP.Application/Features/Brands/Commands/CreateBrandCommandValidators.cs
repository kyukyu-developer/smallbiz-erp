
using FluentValidation;
namespace ERP.Application.Features.Brands.Commands
{
    public class CreateBrandCommandValidators : AbstractValidator<CreateBrandCommand>
    {
        public CreateBrandCommandValidators()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Brand name is required")
                .MaximumLength(50).WithMessage("Brand name cannot exceed 50 characters");

          
        }
    }
}
