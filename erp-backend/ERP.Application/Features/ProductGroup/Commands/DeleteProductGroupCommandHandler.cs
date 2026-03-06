
using MediatR;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.ProductGroup.Commands
{
    public class DeleteProductGroupCommandHandler : IRequestHandler<DeleteProductGroupCommand, Result<bool>>
    {
        private readonly IProductGroupRepository _productGroupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductGroupCommandHandler(
            IProductGroupRepository productGroupRepository,
            IUnitOfWork unitOfWork)
        {
            _productGroupRepository = productGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteProductGroupCommand request, CancellationToken cancellationToken)
        {
            var productGroup = await _productGroupRepository.GetByIdAsync(request.Id);
            if (productGroup == null)
            {
                return Result<bool>.Failure($"Product Group with ID '{request.Id}' not found");
            }

          
            // Soft delete
            productGroup.Active = false;
            productGroup.LastAction = "DELETE";
            productGroup.UpdatedAt = DateTime.UtcNow;
            productGroup.UpdatedBy = "System"; // TODO: Get from current user context

            _productGroupRepository.Update(productGroup);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
