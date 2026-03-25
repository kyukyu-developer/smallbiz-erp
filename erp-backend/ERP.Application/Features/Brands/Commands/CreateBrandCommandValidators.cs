
using FluentValidation;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Brands.Commands
{
    public class CreateBrandCommandValidators : AbstractValidator<CreateBrandCommand>
    {
        private readonly IBrandRepository _brandRepository;

        public CreateBrandCommandValidators(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
            
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Brand name is required")
                .MaximumLength(50).WithMessage("Brand name cannot exceed 50 characters")
                .MustAsync(BeUniqueName).WithMessage("A brand with the same name already exists");
        }

        private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
                return true;
                 
            var existingBrand = await _brandRepository.GetByNameAsync(name.Trim());
            return existingBrand == null;
        }
    }
}
