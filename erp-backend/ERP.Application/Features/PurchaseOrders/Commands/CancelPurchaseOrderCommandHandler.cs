using MediatR;
using ERP.Application.DTOs.Common;
using ERP.Domain.Enums;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.PurchaseOrders.Commands
{
    public class CancelPurchaseOrderCommandHandler : IRequestHandler<CancelPurchaseOrderCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CancelPurchaseOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(CancelPurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.PurchaseOrders.GetByIdAsync(request.Id);
            if (order == null)
                return Result<bool>.Failure("Purchase order not found.");

            if (order.Status == (int)PurchOrderStatus.Cancelled)
                return Result<bool>.Failure("Purchase order is already cancelled.");

            if (order.Status == (int)PurchOrderStatus.PartiallyReceived || order.Status == (int)PurchOrderStatus.FullyReceived)
                return Result<bool>.Failure("Cannot cancel a purchase order that has received goods.");

            order.Status = (int)PurchOrderStatus.Cancelled;
            order.UpdatedAt = DateTime.UtcNow;
            order.LastAction = "CANCEL";

            _unitOfWork.PurchaseOrders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
