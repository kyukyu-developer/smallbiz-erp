using MediatR;
using ERP.Application.DTOs.Purchases;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Purchases.Queries
{
    public class GetPurchasesQueryHandler : IRequestHandler<GetPurchasesQuery, Result<List<PurchaseDto>>>
    {
        private readonly IPurchaseRepository _purchaseRepository;

        public GetPurchasesQueryHandler(IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        public async Task<Result<List<PurchaseDto>>> Handle(GetPurchasesQuery request, CancellationToken cancellationToken)
        {
            var purchases = await _purchaseRepository.GetAllWithDetailsAsync();

            var filteredPurchases = purchases
                .Where(p => !request.SupplierId.HasValue || p.SupplierId == request.SupplierId)
                .Where(p => !request.StartDate.HasValue || p.PurchaseDate >= request.StartDate)
                .Where(p => !request.EndDate.HasValue || p.PurchaseDate <= request.EndDate)
                .Where(p => !request.Status.HasValue || p.Status == request.Status)
                .Where(p => !request.PaymentStatus.HasValue || p.PaymentStatus == request.PaymentStatus)
                .Select(p => new PurchaseDto
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
                    PaymentStatus = p.PaymentStatus,
                    Status = p.Status,
                    ExpectedDate = p.ExpectedDate,
                    ReceivedDate = p.ReceivedDate,
                    Notes = p.Notes,
                    Items = p.Items.Select(i => new PurchaseItemDto
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
                })
                .ToList();

            return Result<List<PurchaseDto>>.Success(filteredPurchases);
        }
    }
}
