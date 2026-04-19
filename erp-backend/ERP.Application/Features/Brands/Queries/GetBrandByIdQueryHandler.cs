
using MediatR;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;
using ERP.Application.DTOs.Brands;

namespace ERP.Application.Features.Brands.Queries
{
    public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, Result<BrandDto>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ICacheService _cache;
        private readonly ICacheKeyBuilder _keyBuilder;

        public GetBrandByIdQueryHandler(
            IBrandRepository brandRepository,
            ICacheService cache,
            ICacheKeyBuilder keyBuilder)
        {
            _brandRepository = brandRepository;
            _cache = cache;
            _keyBuilder = keyBuilder;
        }

        public async Task<Result<BrandDto>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var brands = await _cache.GetOrSetAsync(
                _keyBuilder.Brand_All,
                () => _brandRepository.GetAllAsync(),
                TimeSpan.FromMinutes(15),
                cancellationToken
            );

            var brand = brands.FirstOrDefault(b => b.Id == request.Id);
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
