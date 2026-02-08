using ERP.Shared.Contracts.Common;
using MediatR;
using Sales.Application.DTOs.Sales;
using Sales.Domain.Interfaces;

namespace Sales.Application.Features.Sales.Queries;

public class GetSalesQuery : IRequest<Result<List<SaleDto>>>
{
}

public class GetSalesQueryHandler : IRequestHandler<GetSalesQuery, Result<List<SaleDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSalesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<SaleDto>>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
    {
        var sales = await _unitOfWork.Sales.GetAllAsync();

        var result = new List<SaleDto>();

        foreach (var sale in sales)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(sale.CustomerId);

            result.Add(new SaleDto
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
            });
        }

        return Result<List<SaleDto>>.Success(result);
    }
}
