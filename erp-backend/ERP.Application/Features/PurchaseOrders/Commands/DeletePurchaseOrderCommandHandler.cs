using MediatR;
using ERP.Application.DTOs.Common;
using ERP.Domain.Enums;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.PurchaseOrders.Commands
{
    public class DeletePurchaseOrderCommandHandler : IRequestHandler<DeletePurchaseOrderCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePurchaseOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeletePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.PurchaseOrders.GetByIdAsync(request.Id);
            if (order == null)
                return Result<bool>.Failure("Purchase order not found.");

            if (order.Status != (int)PurchOrderStatus.Draft)
                return Result<bool>.Failure("Only draft purchase orders can be deleted.");

            order.Active = false;
            order.UpdatedAt = DateTime.UtcNow;
            order.LastAction = "DELETE";

            _unitOfWork.PurchaseOrders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
