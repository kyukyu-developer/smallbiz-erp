using ERP.Shared.Contracts.Common;
using MediatR;
using Purchasing.Application.DTOs.Purchases;
using Purchasing.Domain.Interfaces;

namespace Purchasing.Application.Features.Purchases.Queries;

public class GetPurchasesQuery : IRequest<Result<List<PurchaseDto>>>
{
}

public class GetPurchasesQueryHandler : IRequestHandler<GetPurchasesQuery, Result<List<PurchaseDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPurchasesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<PurchaseDto>>> Handle(GetPurchasesQuery request, CancellationToken cancellationToken)
    {
        var purchases = await _unitOfWork.Purchases.GetAllAsync();

        var result = new List<PurchaseDto>();

        foreach (var p in purchases)
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(p.SupplierId);

            result.Add(new PurchaseDto
            {
                Id = p.Id,
                PurchaseOrderNumber = p.PurchaseOrderNumber,
                PurchaseDate = p.PurchaseDate,
                SupplierId = p.SupplierId,
                SupplierName = supplier?.Name ?? string.Empty,
                WarehouseId = p.WarehouseId,
                SubTotal = p.SubTotal,
                TotalDiscount = p.TotalDiscount,
                TotalTax = p.TotalTax,
                TotalAmount = p.TotalAmount,
                PaidAmount = p.PaidAmount,
                PaymentStatus = p.PaymentStatus,
                Status = p.Status,
                Items = new List<PurchaseItemDto>()
            });
        }

        return Result<List<PurchaseDto>>.Success(result);
    }
}
