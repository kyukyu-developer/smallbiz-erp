using ERP.Shared.Contracts.Common;
using MediatR;
using Purchasing.Application.DTOs.Purchases;
using Purchasing.Domain.Entities;
using Purchasing.Domain.Enums;
using Purchasing.Domain.Interfaces;

namespace Purchasing.Application.Features.Purchases.Commands;

public class CreatePurchaseCommand : IRequest<Result<PurchaseDto>>
{
    public CreatePurchaseDto Dto { get; set; } = null!;
}

public class CreatePurchaseCommandHandler : IRequestHandler<CreatePurchaseCommand, Result<PurchaseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreatePurchaseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PurchaseDto>> Handle(CreatePurchaseCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        // Validate supplier exists
        var supplier = await _unitOfWork.Suppliers.GetByIdAsync(dto.SupplierId);
        if (supplier == null)
            return Result<PurchaseDto>.Failure("Supplier not found.");

        // Auto-generate PO number
        var poNumber = $"PO-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";

        // Build purchase items
        var items = dto.Items.Select(i =>
        {
            var discount = i.Discount ?? 0;
            var tax = i.Tax ?? 0;
            var totalCost = (i.Quantity * i.UnitCost) - discount + tax;

            return new PurchaseItem
            {
                ProductId = i.ProductId,
                UnitId = i.UnitId,
                Quantity = i.Quantity,
                UnitCost = i.UnitCost,
                Discount = i.Discount,
                Tax = i.Tax,
                TotalCost = totalCost
            };
        }).ToList();

        var subTotal = items.Sum(i => i.Quantity * i.UnitCost);
        var totalDiscount = items.Sum(i => i.Discount ?? 0);
        var totalTax = items.Sum(i => i.Tax ?? 0);
        var totalAmount = subTotal - totalDiscount + totalTax;

        var purchase = new Purchase
        {
            PurchaseOrderNumber = poNumber,
            PurchaseDate = dto.PurchaseDate,
            SupplierId = dto.SupplierId,
            WarehouseId = dto.WarehouseId,
            SubTotal = subTotal,
            TotalDiscount = totalDiscount,
            TotalTax = totalTax,
            TotalAmount = totalAmount,
            PaidAmount = 0,
            PaymentStatus = PaymentStatus.Unpaid,
            Status = PurchaseStatus.Draft,
            ExpectedDate = dto.ExpectedDate,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow,
            Items = items
        };

        await _unitOfWork.Purchases.AddAsync(purchase);
        await _unitOfWork.SaveChangesAsync();

        var result = new PurchaseDto
        {
            Id = purchase.Id,
            PurchaseOrderNumber = purchase.PurchaseOrderNumber,
            PurchaseDate = purchase.PurchaseDate,
            SupplierId = purchase.SupplierId,
            SupplierName = supplier.Name,
            WarehouseId = purchase.WarehouseId,
            SubTotal = purchase.SubTotal,
            TotalDiscount = purchase.TotalDiscount,
            TotalTax = purchase.TotalTax,
            TotalAmount = purchase.TotalAmount,
            PaidAmount = purchase.PaidAmount,
            PaymentStatus = purchase.PaymentStatus,
            Status = purchase.Status,
            Items = purchase.Items.Select(i => new PurchaseItemDto
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
