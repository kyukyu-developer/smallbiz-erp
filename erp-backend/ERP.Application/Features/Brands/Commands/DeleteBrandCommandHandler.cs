

using MediatR;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Brands.Commands
{
    public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, Result<bool>>
    {
        private readonly IBrandRepository _brandRepository;

        public DeleteBrandCommandHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<Result<bool>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetByIdAsync(request.Id);
            if (brand == null)
            {
                return Result<bool>.Failure("Brand not found");
            }

            _brandRepository.Delete(brand);
            await _brandRepository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
