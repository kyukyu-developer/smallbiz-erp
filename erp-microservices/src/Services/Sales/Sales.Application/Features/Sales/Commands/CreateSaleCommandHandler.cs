using ERP.Shared.Contracts.Common;
using ERP.Shared.Contracts.Events;
using ERP.Shared.MessageBus;
using MediatR;
using Sales.Application.DTOs.Sales;
using Sales.Application.Interfaces;
using Sales.Domain.Entities;
using Sales.Domain.Enums;
using Sales.Domain.Interfaces;

namespace Sales.Application.Features.Sales.Commands;

public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, Result<SaleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IInventoryService _inventoryService;
    private readonly IMessageBus _messageBus;

    public CreateSaleCommandHandler(
        IUnitOfWork unitOfWork,
        IInventoryService inventoryService,
        IMessageBus messageBus)
    {
        _unitOfWork = unitOfWork;
        _inventoryService = inventoryService;
        _messageBus = messageBus;
    }

    public async Task<Result<SaleDto>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        // Validate customer exists
        var customer = await _unitOfWork.Customers.GetByIdAsync(dto.CustomerId);
        if (customer == null)
            return Result<SaleDto>.Failure("Customer not found.");

        // Validate stock availability if WarehouseId is provided
        if (dto.WarehouseId.HasValue)
        {
            foreach (var item in dto.Items)
            {
                var isAvailable = await _inventoryService.CheckStockAvailability(
                    item.ProductId, dto.WarehouseId.Value, item.Quantity);

                if (!isAvailable)
                    return Result<SaleDto>.Failure(
                        $"Insufficient stock for product {item.ProductId} in warehouse {dto.WarehouseId.Value}.");
            }
        }

        // Calculate totals
        var saleItems = new List<SalesItem>();
        decimal subTotal = 0;
        decimal totalDiscount = 0;
        decimal totalTax = 0;

        foreach (var itemDto in dto.Items)
        {
            var lineTotal = itemDto.Quantity * itemDto.UnitPrice;
            var discount = itemDto.Discount ?? 0;
            var tax = itemDto.Tax ?? 0;
            var totalPrice = lineTotal - discount + tax;

            subTotal += lineTotal;
            totalDiscount += discount;
            totalTax += tax;

            saleItems.Add(new SalesItem
            {
                ProductId = itemDto.ProductId,
                UnitId = itemDto.UnitId,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice,
                Discount = itemDto.Discount,
                Tax = itemDto.Tax,
                TotalPrice = totalPrice
            });
        }

        var totalAmount = subTotal - totalDiscount + totalTax;

        // Generate invoice number
        var invoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

        var sale = new Sale
        {
            InvoiceNumber = invoiceNumber,
            SaleDate = dto.SaleDate,
            CustomerId = dto.CustomerId,
            WarehouseId = dto.WarehouseId,
            SubTotal = subTotal,
            TotalDiscount = totalDiscount > 0 ? totalDiscount : null,
            TotalTax = totalTax > 0 ? totalTax : null,
            TotalAmount = totalAmount,
            PaidAmount = 0,
            PaymentStatus = PaymentStatus.Unpaid,
            Status = SaleStatus.Confirmed,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow,
            Items = saleItems
        };

        await _unitOfWork.Sales.AddAsync(sale);
        await _unitOfWork.SaveChangesAsync();

        // Publish SaleCreatedEvent
        var saleCreatedEvent = new SaleCreatedEvent
        {
            SaleId = sale.Id,
            InvoiceNumber = sale.InvoiceNumber,
            CustomerId = sale.CustomerId,
            WarehouseId = sale.WarehouseId,
            TotalAmount = sale.TotalAmount,
            Items = sale.Items.Select(i => new SaleItemEvent
            {
                ProductId = i.ProductId,
                UnitId = i.UnitId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        _messageBus.Publish(saleCreatedEvent);

        // Map to DTO
        var result = new SaleDto
        {
            Id = sale.Id,
            InvoiceNumber = sale.InvoiceNumber,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            CustomerName = customer.Name,
            WarehouseId = sale.WarehouseId,
            SubTotal = sale.SubTotal,
            TotalDiscount = sale.TotalDiscount,
            TotalTax = sale.TotalTax,
            TotalAmount = sale.TotalAmount,
            PaidAmount = sale.PaidAmount,
            PaymentStatus = sale.PaymentStatus.ToString(),
            Status = sale.Status.ToString(),
            Items = sale.Items.Select(i => new SaleItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                UnitId = i.UnitId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                Tax = i.Tax,
                TotalPrice = i.TotalPrice
            }).ToList()
        };

        return Result<SaleDto>.Success(result);
    }
}
