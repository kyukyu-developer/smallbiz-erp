using MediatR;
using ERP.Application.DTOs.PurchaseOrders;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.PurchaseOrders.Commands
{
    public class CreatePurchaseOrderCommandHandler : IRequestHandler<CreatePurchaseOrderCommand, Result<PurchaseOrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePurchaseOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PurchaseOrderDto>> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(request.SupplierId);
            if (supplier == null)
                return Result<PurchaseOrderDto>.Failure("Supplier not found.");

            var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(request.WarehouseId);
            if (warehouse == null)
                return Result<PurchaseOrderDto>.Failure("Warehouse not found.");

            // Generate PO number
            var orderNumber = await GenerateOrderNumberAsync();

            // Calculate line items
            decimal subTotal = 0, totalDiscount = 0, totalTax = 0;
            var orderItems = new List<PurchOrderItem>();

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

                orderItems.Add(new PurchOrderItem
                {
                    Id = Guid.NewGuid().ToString(),
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

            var order = new PurchOrder
            {
                Id = Guid.NewGuid().ToString(),
                OrderNumber = orderNumber,
                OrderDate = request.OrderDate,
                SupplierId = request.SupplierId,
                WarehouseId = request.WarehouseId,
                SubTotal = subTotal,
                TotalDiscount = totalDiscount,
                TotalTax = totalTax,
                TotalAmount = subTotal - totalDiscount + totalTax,
                Status = (int)PurchOrderStatus.Draft,
                ExpectedDate = request.ExpectedDate,
                Notes = request.Notes,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                LastAction = "CREATE",
                PurchOrderItem = orderItems
            };

            await _unitOfWork.PurchaseOrders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return Result<PurchaseOrderDto>.Success(MapToDto(order, supplier.Name, warehouse.Name));
        }

        private async Task<string> GenerateOrderNumberAsync()
        {
            var year = DateTime.UtcNow.Year;
            var allOrders = await _unitOfWork.PurchaseOrders.GetAllAsync();
            var count = allOrders.Count(o => o.CreatedAt.Year == year) + 1;
            return $"PO-{year}{count:D4}";
        }

        internal static PurchaseOrderDto MapToDto(PurchOrder o, string? supplierName = null, string? warehouseName = null) => new()
        {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            OrderDate = o.OrderDate,
            SupplierId = o.SupplierId,
            SupplierName = supplierName ?? o.Supplier?.Name,
            WarehouseId = o.WarehouseId,
            WarehouseName = warehouseName ?? o.Warehouse?.Name,
            SubTotal = o.SubTotal,
            TotalDiscount = o.TotalDiscount,
            TotalTax = o.TotalTax,
            TotalAmount = o.TotalAmount,
            Status = (PurchOrderStatus)o.Status,
            ExpectedDate = o.ExpectedDate,
            Notes = o.Notes,
            CreatedAt = o.CreatedAt,
            CreatedBy = o.CreatedBy,
            Items = o.PurchOrderItem?.Select(i => new PurchaseOrderItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product?.Name,
                ProductCode = i.Product?.Code,
                UnitId = i.UnitId,
                UnitName = i.Unit?.Name,
                Quantity = i.Quantity,
                ReceivedQuantity = i.ReceivedQuantity,
                UnitCost = i.UnitCost,
                DiscountPercent = i.DiscountPercent,
                DiscountAmount = i.DiscountAmount,
                TaxPercent = i.TaxPercent,
                TaxAmount = i.TaxAmount,
                TotalAmount = i.TotalAmount,
                Notes = i.Notes
            }).ToList() ?? new()
        };
    }
}
