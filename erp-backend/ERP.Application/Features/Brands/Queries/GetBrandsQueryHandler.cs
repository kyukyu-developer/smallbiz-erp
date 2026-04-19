
﻿using ERP.Application.DTOs.Brands;
using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.Units;
using ERP.Domain.Interfaces;
using MediatR;

namespace ERP.Application.Features.Brands.Queries
{

    public class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, Result<List<BrandDto>>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ICacheService _cache;
        private readonly ICacheKeyBuilder _keyBuilder;

        public GetBrandsQueryHandler(
            IBrandRepository brandRepository,
            ICacheService cache,
            ICacheKeyBuilder keyBuilder)
        {
            _brandRepository = brandRepository;
            _cache = cache;
            _keyBuilder = keyBuilder;
        }

        public async Task<Result<List<BrandDto>>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _cache.GetOrSetAsync(
                _keyBuilder.Brand_All,
                () => _brandRepository.GetAllAsync(),
                TimeSpan.FromMinutes(15),
                cancellationToken
            );

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
