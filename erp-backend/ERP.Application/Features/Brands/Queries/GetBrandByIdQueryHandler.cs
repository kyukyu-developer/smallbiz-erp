
using MediatR;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;
using ERP.Application.DTOs.Brands;

namespace ERP.Application.Features.Brands.Queries
{
    public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, Result<BrandDto>>
    {
        private readonly IBrandRepository _brandRepository;

        public GetBrandByIdQueryHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<Result<BrandDto>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetByIdAsync(request.Id);
            if (brand == null)
            {
                return Result<BrandDto>.Failure("Brand not found");
            }

            var BrandDto = new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description,
                Active = brand.Active
            };

            return Result<BrandDto>.Success(BrandDto);
        }
    }
}
