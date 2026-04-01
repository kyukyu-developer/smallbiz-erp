using MediatR;
using ERP.Application.DTOs.PurchaseOrders;
using ERP.Application.DTOs.Common;
using ERP.Domain.Enums;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.PurchaseOrders.Commands
{
    public class ApprovePurchaseOrderCommandHandler : IRequestHandler<ApprovePurchaseOrderCommand, Result<PurchaseOrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApprovePurchaseOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PurchaseOrderDto>> Handle(ApprovePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.PurchaseOrders.GetByIdAsync(request.Id);
            if (order == null)
                return Result<PurchaseOrderDto>.Failure("Purchase order not found.");

            if (order.Status != (int)PurchOrderStatus.Draft)
                return Result<PurchaseOrderDto>.Failure("Only draft purchase orders can be approved.");

            order.Status = (int)PurchOrderStatus.Approved;
            order.UpdatedAt = DateTime.UtcNow;
            order.LastAction = "APPROVE";

            _unitOfWork.PurchaseOrders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(order.SupplierId);
            var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(order.WarehouseId);

            return Result<PurchaseOrderDto>.Success(
                CreatePurchaseOrderCommandHandler.MapToDto(order, supplier?.Name, warehouse?.Name));
        }
    }
}
