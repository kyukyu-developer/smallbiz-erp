using ERP.Shared.Contracts.Common;
using ERP.Shared.Contracts.Events;
using ERP.Shared.MessageBus;
using MediatR;
using Purchasing.Application.DTOs.Purchases;
using Purchasing.Domain.Enums;
using Purchasing.Domain.Interfaces;

namespace Purchasing.Application.Features.Purchases.Commands;

public class ReceivePurchaseCommand : IRequest<Result<PurchaseDto>>
{
    public int PurchaseId { get; set; }
    public ReceivePurchaseDto Dto { get; set; } = null!;
}

public class ReceivePurchaseCommandHandler : IRequestHandler<ReceivePurchaseCommand, Result<PurchaseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBus _messageBus;

    public ReceivePurchaseCommandHandler(IUnitOfWork unitOfWork, IMessageBus messageBus)
    {
        _unitOfWork = unitOfWork;
        _messageBus = messageBus;
    }

    public async Task<Result<PurchaseDto>> Handle(ReceivePurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = await _unitOfWork.Purchases.GetByIdAsync(request.PurchaseId);
        if (purchase == null)
            return Result<PurchaseDto>.Failure("Purchase order not found.");

        if (purchase.Status == PurchaseStatus.Received)
            return Result<PurchaseDto>.Failure("Purchase order has already been received.");

        if (purchase.Status == PurchaseStatus.Cancelled)
            return Result<PurchaseDto>.Failure("Cannot receive a cancelled purchase order.");

        // Load items
        var items = await _unitOfWork.PurchaseItems.FindAsync(i => i.PurchaseId == purchase.Id);
        var itemList = items.ToList();

        // Load supplier
        var supplier = await _unitOfWork.Suppliers.GetByIdAsync(purchase.SupplierId);

        // Mark as received
        purchase.Status = PurchaseStatus.Received;
        purchase.ReceivedDate = request.Dto.ReceivedDate;
        if (request.Dto.Notes != null)
            purchase.Notes = request.Dto.Notes;
        purchase.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Purchases.Update(purchase);
        await _unitOfWork.SaveChangesAsync();

        // Publish PurchaseReceivedEvent via IMessageBus
        var purchaseReceivedEvent = new PurchaseReceivedEvent
        {
            PurchaseId = purchase.Id,
            PurchaseOrderNumber = purchase.PurchaseOrderNumber,
            SupplierId = purchase.SupplierId,
            WarehouseId = purchase.WarehouseId,
            Items = itemList.Select(i => new PurchaseItemEvent
            {
                ProductId = i.ProductId,
                UnitId = i.UnitId,
                Quantity = i.Quantity,
                UnitCost = i.UnitCost
            }).ToList()
        };

        _messageBus.Publish(purchaseReceivedEvent);

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
            Items = itemList.Select(i => new PurchaseItemDto
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
