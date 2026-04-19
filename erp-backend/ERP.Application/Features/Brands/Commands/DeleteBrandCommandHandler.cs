

using MediatR;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Brands.Commands
{
    public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, Result<bool>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;
        private readonly ICacheKeyBuilder _keyBuilder;
  
        public DeleteBrandCommandHandler(
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
  
        public async Task<Result<bool>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetByIdAsync(request.Id);
            if (brand == null)
            {
                return Result<bool>.Failure("Brand not found");
            }
  
            _brandRepository.Delete(brand);
            await _unitOfWork.SaveChangesAsync();

            await _cache.InvalidateCacheAsync(_keyBuilder.Brand_All, cancellationToken);
  
            return Result<bool>.Success(true);
        }
    }
}
