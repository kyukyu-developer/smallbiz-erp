
using MediatR;
using ERP.Application.DTOs.Categories;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;
using ERP.Application.DTOs.Brands;

namespace ERP.Application.Features.Brands.Commands
{
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, Result<BrandDto>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<BrandDto>> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetByIdAsync(request.Id);
            if (brand == null)
            {
                return Result<BrandDto>.Failure("Brand not found");
            }

            brand.Name = request.Name;
            brand.Description = request.Description;
            brand.Active = request.Active;

            _brandRepository.Update(brand);
            await _unitOfWork.SaveChangesAsync();

            var brandDto = new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description,
                Active = brand.Active
            };

            return Result<BrandDto>.Success(brandDto);
        }
    }
}
