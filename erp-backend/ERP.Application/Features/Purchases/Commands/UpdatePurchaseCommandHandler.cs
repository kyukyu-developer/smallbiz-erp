using MediatR;
using ERP.Application.DTOs.Purchases;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Purchases.Commands
{
    public class UpdatePurchaseCommandHandler : IRequestHandler<UpdatePurchaseCommand, Result<PurchaseDto>>
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePurchaseCommandHandler(IPurchaseRepository purchaseRepository, IUnitOfWork unitOfWork)
        {
            _purchaseRepository = purchaseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PurchaseDto>> Handle(UpdatePurchaseCommand request, CancellationToken cancellationToken)
        {
            var purchase = await _purchaseRepository.GetByIdWithDetailsAsync(request.Id);
            if (purchase == null)
                return Result<PurchaseDto>.Failure("Purchase invoice not found.");

            if (purchase.Status == (int)Domain.Enums.PurchaseStatus.Cancelled)
                return Result<PurchaseDto>.Failure("Cannot update a cancelled purchase invoice.");

            // Update header
            purchase.PurchaseDate = request.PurchaseDate;
            purchase.SupplierId = request.SupplierId;
            purchase.WarehouseId = request.WarehouseId;
            purchase.ExpectedDate = request.ExpectedDate;
            purchase.Notes = request.Notes;
            purchase.UpdatedAt = DateTime.UtcNow;
            purchase.LastAction = "UPDATE";

            // Remove old items
            var oldItems = purchase.PurchItem.ToList();
            foreach (var item in oldItems)
            {
                _unitOfWork.PurchaseItems.Remove(item);
            }

            // Recalculate and add new items
            decimal subTotal = 0;
            decimal totalDiscount = 0;
            decimal totalTax = 0;
            var newItems = new List<PurchItem>();

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

                newItems.Add(new PurchItem
                {
                    Id = Guid.NewGuid().ToString(),
                    PurchaseId = purchase.Id,
                    ProductId = itemDto.ProductId,
                    UnitId = itemDto.UnitId,
                    Quantity = itemDto.Quantity,
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

            await _unitOfWork.PurchaseItems.AddRangeAsync(newItems);

            purchase.SubTotal = subTotal;
            purchase.TotalDiscount = totalDiscount;
            purchase.TotalTax = totalTax;
            purchase.TotalAmount = subTotal - totalDiscount + totalTax;

            _purchaseRepository.Update(purchase);
            await _unitOfWork.SaveChangesAsync();

            // Return updated DTO
            var updated = await _purchaseRepository.GetByIdWithDetailsAsync(purchase.Id);
            return Result<PurchaseDto>.Success(MapToDto(updated!));
        }

        private static PurchaseDto MapToDto(PurchInvoice p) => new()
        {
            Id = p.Id,
            PurchaseOrderNumber = p.PurchaseOrderNumber,
            PurchaseDate = p.PurchaseDate,
            SupplierId = p.SupplierId,
            SupplierName = p.Supplier?.Name,
            WarehouseId = p.WarehouseId,
            SubTotal = p.SubTotal,
            TotalDiscount = p.TotalDiscount,
            TotalTax = p.TotalTax,
            TotalAmount = p.TotalAmount,
            PaidAmount = p.PaidAmount,
            PaymentStatus = (Domain.Enums.PaymentStatus)p.PaymentStatus,
            Status = (Domain.Enums.PurchaseStatus)p.Status,
            ExpectedDate = p.ExpectedDate,
            ReceivedDate = p.ReceivedDate,
            Notes = p.Notes,
            Items = p.PurchItem.Select(i => new PurchaseItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product?.Name,
                UnitId = i.UnitId,
                Quantity = i.Quantity,
                UnitCost = i.UnitCost,
                DiscountPercent = i.DiscountPercent,
                DiscountAmount = i.DiscountAmount,
                TaxPercent = i.TaxPercent,
                TaxAmount = i.TaxAmount,
                TotalAmount = i.TotalAmount,
                Notes = i.Notes
            }).ToList()
        };
    }
}
