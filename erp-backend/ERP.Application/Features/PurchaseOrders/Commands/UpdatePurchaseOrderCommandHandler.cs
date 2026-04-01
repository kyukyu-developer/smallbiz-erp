using MediatR;
using ERP.Application.DTOs.PurchaseOrders;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.PurchaseOrders.Commands
{
    public class UpdatePurchaseOrderCommandHandler : IRequestHandler<UpdatePurchaseOrderCommand, Result<PurchaseOrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePurchaseOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PurchaseOrderDto>> Handle(UpdatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.PurchaseOrders.GetByIdAsync(request.Id);
            if (order == null)
                return Result<PurchaseOrderDto>.Failure("Purchase order not found.");

            if (order.Status != (int)PurchOrderStatus.Draft)
                return Result<PurchaseOrderDto>.Failure("Only draft purchase orders can be updated.");

            // Update header
            order.OrderDate = request.OrderDate;
            order.SupplierId = request.SupplierId;
            order.WarehouseId = request.WarehouseId;
            order.ExpectedDate = request.ExpectedDate;
            order.Notes = request.Notes;
            order.UpdatedAt = DateTime.UtcNow;
            order.LastAction = "UPDATE";

            // Remove old items
            var oldItems = await _unitOfWork.PurchaseOrderItems.FindAsync(i => i.PurchaseOrderId == order.Id);
            foreach (var item in oldItems)
                _unitOfWork.PurchaseOrderItems.Remove(item);

            // Recalculate and add new items
            decimal subTotal = 0, totalDiscount = 0, totalTax = 0;

            foreach (var itemDto in request.Items)
            {
                var lineTotal = itemDto.Quantity * itemDto.UnitCost;
                var discountAmount = itemDto.DiscountAmount ?? 0;
                if (itemDto.DiscountPercent.HasValue)
                    discountAmount = lineTotal * (itemDto.DiscountPercent.Value / 100);

                var taxableAmount = lineTotal - discountAmount;
                var taxAmount = itemDto.TaxAmount ?? 0;
                if (itemDto.TaxPercent.HasValue)
                    taxAmount = taxableAmount * (itemDto.TaxPercent.Value / 100);

                var itemTotal = taxableAmount + taxAmount;
                subTotal += lineTotal;
                totalDiscount += discountAmount;
                totalTax += taxAmount;

                await _unitOfWork.PurchaseOrderItems.AddAsync(new PurchOrderItem
                {
                    Id = Guid.NewGuid().ToString(),
                    PurchaseOrderId = order.Id,
                    ProductId = itemDto.ProductId,
                    UnitId = itemDto.UnitId,
                    Quantity = itemDto.Quantity,
                    ReceivedQuantity = 0,
                    UnitCost = itemDto.UnitCost,
                    DiscountPercent = itemDto.DiscountPercent,
                    DiscountAmount = discountAmount,
                    TaxPercent = itemDto.TaxPercent,
                    TaxAmount = taxAmount,
                    TotalAmount = itemTotal,
                    Notes = itemDto.Notes,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    LastAction = "CREATE"
                });
            }

            order.SubTotal = subTotal;
            order.TotalDiscount = totalDiscount;
            order.TotalTax = totalTax;
            order.TotalAmount = subTotal - totalDiscount + totalTax;

            _unitOfWork.PurchaseOrders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(order.SupplierId);
            var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(order.WarehouseId);

            return Result<PurchaseOrderDto>.Success(
                CreatePurchaseOrderCommandHandler.MapToDto(order, supplier?.Name, warehouse?.Name));
        }
    }
}
