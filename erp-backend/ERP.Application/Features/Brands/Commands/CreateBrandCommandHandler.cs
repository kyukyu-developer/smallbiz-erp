
using MediatR;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;
using ERP.Application.DTOs.Brands;

namespace ERP.Application.Features.Brands.Commands
{
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Result<BrandDto>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;
        private readonly ICacheKeyBuilder _keyBuilder;


        public CreateBrandCommandHandler(
            IBrandRepository brandRepository, 
            IUnitOfWork unitOfWork,
            ICacheService cache,
            ICacheKeyBuilder keyBuilder)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
            _cache = cache;
            _keyBuilder = keyBuilder;
        }

        public async Task<Result<BrandDto>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = new Domain.Entities.ProdBrand
            {
                Id = Guid.NewGuid().ToString(),

                Name = request.Name,
                Description = request.Description,
                Active = request.Active,
                  LastAction = "CREATE",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };

            await _brandRepository.AddAsync(brand);
            await _unitOfWork.SaveChangesAsync();

            await _cache.InvalidateCacheAsync(_keyBuilder.Brand_All, cancellationToken);

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
