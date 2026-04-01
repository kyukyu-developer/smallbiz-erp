using MediatR;
using ERP.Application.DTOs.PurchaseOrders;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.PurchaseOrders.Queries
{
    public class GetPurchaseOrderByIdQueryHandler : IRequestHandler<GetPurchaseOrderByIdQuery, Result<PurchaseOrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPurchaseOrderByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PurchaseOrderDto>> Handle(GetPurchaseOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.PurchaseOrders.GetByIdAsync(request.Id);
            if (order == null)
                return Result<PurchaseOrderDto>.Failure("Purchase order not found.");

            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(order.SupplierId);
            var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(order.WarehouseId);

            // Load items
            var items = await _unitOfWork.PurchaseOrderItems.FindAsync(i => i.PurchaseOrderId == order.Id);
            order.PurchOrderItem = items.ToList();

            return Result<PurchaseOrderDto>.Success(
                Features.PurchaseOrders.Commands.CreatePurchaseOrderCommandHandler.MapToDto(order, supplier?.Name, warehouse?.Name));
        }
    }
}
