using MediatR;
using ERP.Application.DTOs.Purchases;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Purchases.Queries
{
    public class GetPurchaseByIdQueryHandler : IRequestHandler<GetPurchaseByIdQuery, Result<PurchaseDto>>
    {
        private readonly IPurchaseRepository _purchaseRepository;

        public GetPurchaseByIdQueryHandler(IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        public async Task<Result<PurchaseDto>> Handle(GetPurchaseByIdQuery request, CancellationToken cancellationToken)
        {
            var purchase = await _purchaseRepository.GetByIdWithDetailsAsync(request.Id);
            if (purchase == null)
            {
                return Result<PurchaseDto>.Failure("Purchase not found");
            }

            var purchaseDto = new PurchaseDto
            {
                Id = purchase.Id,
                PurchaseOrderNumber = purchase.PurchaseOrderNumber,
                PurchaseDate = purchase.PurchaseDate,
                SupplierId = purchase.SupplierId,
                SupplierName = purchase.Supplier?.Name,
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

            return Result<PurchaseDto>.Success(purchaseDto);
        }
    }
}
