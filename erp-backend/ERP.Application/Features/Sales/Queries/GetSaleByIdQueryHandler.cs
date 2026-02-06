using MediatR;
using ERP.Application.DTOs.Sales;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Sales.Queries
{
    public class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, Result<SaleDto>>
    {
        private readonly ISaleRepository _saleRepository;

        public GetSaleByIdQueryHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<Result<SaleDto>> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetByIdWithDetailsAsync(request.Id);
            if (sale == null)
            {
                return Result<SaleDto>.Failure("Sale not found");
            }

            var saleDto = new SaleDto
            {
                Id = sale.Id,
                InvoiceNumber = sale.InvoiceNumber,
                SaleDate = sale.SaleDate,
                CustomerId = sale.CustomerId,
                CustomerName = sale.Customer?.Name,
                WarehouseId = sale.WarehouseId,
                SubTotal = sale.SubTotal,
                TotalDiscount = sale.TotalDiscount,
                TotalTax = sale.TotalTax,
                TotalAmount = sale.TotalAmount,
                PaidAmount = sale.PaidAmount,
                PaymentStatus = sale.PaymentStatus,
                Status = sale.Status,
                DueDate = sale.DueDate,
                Notes = sale.Notes,
                Items = sale.Items.Select(i => new SaleItemDto
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
            };

            return Result<SaleDto>.Success(saleDto);
        }
    }
}
