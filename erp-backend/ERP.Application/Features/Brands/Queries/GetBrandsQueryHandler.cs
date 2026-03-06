
using ERP.Application.DTOs.Brands;
using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.Units;
using ERP.Domain.Interfaces;
using MediatR;

namespace ERP.Application.Features.Brands.Queries
{

    public class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, Result<List<BrandDto>>>
    {
        private readonly IBrandRepository _brandRepository;

        public GetBrandsQueryHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<Result<List<BrandDto>>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Domain.Entities.Brands> brands;

            brands = await _brandRepository.GetAllAsync();


            // Apply active filter
            var filteredBrands = brands
                .Select(w => new BrandDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    Description = w.Description,
                    Active = w.Active
                  
                })
                .ToList();

            return Result<List<BrandDto>>.Success(filteredBrands);
        }
    }

}
