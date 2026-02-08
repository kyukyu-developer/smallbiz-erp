using ERP.Shared.Contracts.Common;
using MediatR;
using Purchasing.Application.DTOs.Purchases;
using Purchasing.Domain.Interfaces;

namespace Purchasing.Application.Features.Purchases.Queries;

public class GetPurchaseByIdQuery : IRequest<Result<PurchaseDto>>
{
    public int Id { get; set; }
}

public class GetPurchaseByIdQueryHandler : IRequestHandler<GetPurchaseByIdQuery, Result<PurchaseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPurchaseByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PurchaseDto>> Handle(GetPurchaseByIdQuery request, CancellationToken cancellationToken)
    {
        var purchase = await _unitOfWork.Purchases.GetByIdAsync(request.Id);
        if (purchase == null)
            return Result<PurchaseDto>.Failure("Purchase order not found.");

        var supplier = await _unitOfWork.Suppliers.GetByIdAsync(purchase.SupplierId);
        var items = await _unitOfWork.PurchaseItems.FindAsync(i => i.PurchaseId == purchase.Id);

        var result = new PurchaseDto
        {
            Id = purchase.Id,
            PurchaseOrderNumber = purchase.PurchaseOrderNumber,
            PurchaseDate = purchase.PurchaseDate,
            SupplierId = purchase.SupplierId,
            SupplierName = supplier?.Name ?? string.Empty,
            WarehouseId = purchase.WarehouseId,
            SubTotal = purchase.SubTotal,
            TotalDiscount = purchase.TotalDiscount,
            TotalTax = purchase.TotalTax,
            TotalAmount = purchase.TotalAmount,
            PaidAmount = purchase.PaidAmount,
            PaymentStatus = purchase.PaymentStatus,
            Status = purchase.Status,
            Items = items.Select(i => new PurchaseItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                UnitId = i.UnitId,
                Quantity = i.Quantity,
                UnitCost = i.UnitCost,
                Discount = i.Discount,
                Tax = i.Tax,
                TotalCost = i.TotalCost
            }).ToList()
        };

        return Result<PurchaseDto>.Success(result);
    }
}
