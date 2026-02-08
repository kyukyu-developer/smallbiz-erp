using ERP.Shared.Contracts.Common;
using MediatR;
using Sales.Application.DTOs.Sales;
using Sales.Domain.Interfaces;

namespace Sales.Application.Features.Sales.Queries;

public class GetSaleByIdQuery : IRequest<Result<SaleDto>>
{
    public int Id { get; set; }
}

public class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, Result<SaleDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSaleByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<SaleDto>> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        var sale = await _unitOfWork.Sales.GetByIdAsync(request.Id);

        if (sale == null)
            return Result<SaleDto>.Failure("Sale not found.");

        var customer = await _unitOfWork.Customers.GetByIdAsync(sale.CustomerId);

        // Load items
        var items = await _unitOfWork.SalesItems.FindAsync(i => i.SaleId == sale.Id);

        var result = new SaleDto
        {
            Id = sale.Id,
            InvoiceNumber = sale.InvoiceNumber,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            CustomerName = customer?.Name ?? string.Empty,
            WarehouseId = sale.WarehouseId,
            SubTotal = sale.SubTotal,
            TotalDiscount = sale.TotalDiscount,
            TotalTax = sale.TotalTax,
            TotalAmount = sale.TotalAmount,
            PaidAmount = sale.PaidAmount,
            PaymentStatus = sale.PaymentStatus.ToString(),
            Status = sale.Status.ToString(),
            Items = items.Select(i => new SaleItemDto
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
