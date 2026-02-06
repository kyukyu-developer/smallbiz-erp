using MediatR;
using ERP.Application.DTOs.Purchases;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Purchases.Commands
{
    public class CreatePurchaseCommandHandler : IRequestHandler<CreatePurchaseCommand, Result<PurchaseDto>>
    {
        private readonly IPurchaseRepository _purchaseRepository;

        public CreatePurchaseCommandHandler(IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        public async Task<Result<PurchaseDto>> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
        {
            // Calculate totals
            decimal subTotal = 0;
            decimal totalDiscount = 0;
            decimal totalTax = 0;

            var purchaseItems = new List<PurchaseItem>();

            foreach (var itemDto in request.Items)
            {
                var lineTotal = itemDto.Quantity * itemDto.UnitCost;
                var discountAmount = itemDto.DiscountAmount ?? 0;
                if (itemDto.DiscountPercent.HasValue)
                {
                    discountAmount = lineTotal * (itemDto.DiscountPercent.Value / 100);
                }

                var taxableAmount = lineTotal - discountAmount;
                var taxAmount = itemDto.TaxAmount ?? 0;
                if (itemDto.TaxPercent.HasValue)
                {
                    taxAmount = taxableAmount * (itemDto.TaxPercent.Value / 100);
                }

                var itemTotal = taxableAmount + taxAmount;

                subTotal += lineTotal;
                totalDiscount += discountAmount;
                totalTax += taxAmount;

                purchaseItems.Add(new PurchaseItem
                {
                    ProductId = itemDto.ProductId,
                    UnitId = itemDto.UnitId,
                    Quantity = itemDto.Quantity,
                    UnitCost = itemDto.UnitCost,
                    DiscountPercent = itemDto.DiscountPercent,
                    DiscountAmount = discountAmount,
                    TaxPercent = itemDto.TaxPercent,
                    TaxAmount = taxAmount,
                    TotalAmount = itemTotal,
                    Notes = itemDto.Notes
                });
            }

            var purchase = new Purchase
            {
                PurchaseOrderNumber = request.PurchaseOrderNumber,
                PurchaseDate = request.PurchaseDate,
                SupplierId = request.SupplierId,
                WarehouseId = request.WarehouseId,
                SubTotal = subTotal,
                TotalDiscount = totalDiscount,
                TotalTax = totalTax,
                TotalAmount = subTotal - totalDiscount + totalTax,
                PaidAmount = 0,
                PaymentStatus = request.PaymentStatus,
                Status = request.Status,
                ExpectedDate = request.ExpectedDate,
                Notes = request.Notes,
                Items = purchaseItems
            };

            await _purchaseRepository.AddAsync(purchase);
            await _purchaseRepository.SaveChangesAsync();

            var purchaseDto = new PurchaseDto
            {
                Id = purchase.Id,
                PurchaseOrderNumber = purchase.PurchaseOrderNumber,
                PurchaseDate = purchase.PurchaseDate,
                SupplierId = purchase.SupplierId,
                WarehouseId = purchase.WarehouseId,
                SubTotal = purchase.SubTotal,
                TotalDiscount = purchase.TotalDiscount,
                TotalTax = purchase.TotalTax,
                TotalAmount = purchase.TotalAmount,
                PaidAmount = purchase.PaidAmount,
                PaymentStatus = purchase.PaymentStatus,
                Status = purchase.Status,
                ExpectedDate = purchase.ExpectedDate,
                ReceivedDate = purchase.ReceivedDate,
                Notes = purchase.Notes,
                Items = purchase.Items.Select(i => new PurchaseItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
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

            return Result<PurchaseDto>.Success(purchaseDto);
        }
    }
}
