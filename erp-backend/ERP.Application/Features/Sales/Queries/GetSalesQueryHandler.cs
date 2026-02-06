using MediatR;
using ERP.Application.DTOs.Sales;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Sales.Queries
{
    public class GetSalesQueryHandler : IRequestHandler<GetSalesQuery, Result<List<SaleDto>>>
    {
        private readonly ISaleRepository _saleRepository;

        public GetSalesQueryHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<Result<List<SaleDto>>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
        {
            var sales = await _saleRepository.GetAllWithDetailsAsync();

            var filteredSales = sales
                .Where(s => !request.CustomerId.HasValue || s.CustomerId == request.CustomerId)
                .Where(s => !request.StartDate.HasValue || s.SaleDate >= request.StartDate)
                .Where(s => !request.EndDate.HasValue || s.SaleDate <= request.EndDate)
                .Where(s => !request.Status.HasValue || s.Status == request.Status)
                .Where(s => !request.PaymentStatus.HasValue || s.PaymentStatus == request.PaymentStatus)
                .Select(s => new SaleDto
                {
                    Id = s.Id,
                    InvoiceNumber = s.InvoiceNumber,
                    SaleDate = s.SaleDate,
                    CustomerId = s.CustomerId,
                    CustomerName = s.Customer?.Name,
                    WarehouseId = s.WarehouseId,
                    SubTotal = s.SubTotal,
                    TotalDiscount = s.TotalDiscount,
                    TotalTax = s.TotalTax,
                    TotalAmount = s.TotalAmount,
                    PaidAmount = s.PaidAmount,
                    PaymentStatus = s.PaymentStatus,
                    Status = s.Status,
                    DueDate = s.DueDate,
                    Notes = s.Notes,
                    Items = s.Items.Select(i => new SaleItemDto
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        ProductName = i.Product?.Name,
                        UnitId = i.UnitId,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        DiscountPercent = i.DiscountPercent,
                        DiscountAmount = i.DiscountAmount,
                        TaxPercent = i.TaxPercent,
                        TaxAmount = i.TaxAmount,
                        TotalAmount = i.TotalAmount,
                        Notes = i.Notes
                    }).ToList()
                })
                .ToList();

            return Result<List<SaleDto>>.Success(filteredSales);
        }
    }
}
