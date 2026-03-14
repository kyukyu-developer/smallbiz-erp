using MediatR;
using ERP.Application.DTOs.Sales;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Sales.Commands
{
    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, Result<SaleDto>>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;

        public CreateSaleCommandHandler(ISaleRepository saleRepository, IProductRepository productRepository)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
        }

        public async Task<Result<SaleDto>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            // Calculate totals
            decimal subTotal = 0;
            decimal totalDiscount = 0;
            decimal totalTax = 0;

            var saleItems = new List<Domain.Entities.SalesInvoiceItem>();

            foreach (var itemDto in request.Items)
            {
                var lineTotal = itemDto.Quantity * itemDto.UnitPrice;
                var discountAmount = itemDto.DiscountAmount ?? 0;
                if (itemDto.DiscountPercent.HasValue)
                {
                    discountAmount = lineTotal * (itemDto.DiscountPercent.Value / 100);
                }

                var taxableAmount = lineTotal - discountAmount;
                var taxAmount = itemDto.TaxAmount ?? 0;
                if (itemDto.TaxPercent.HasValue)
                {
                    taxAmount = taxableAmount * (itemDto.TaxPercent.Value / 100);
                }

                var itemTotal = taxableAmount + taxAmount;

                subTotal += lineTotal;
                totalDiscount += discountAmount;
                totalTax += taxAmount;

                saleItems.Add(new Domain.Entities.SalesInvoiceItem
                {
                    ProductId = itemDto.ProductId,
                    UnitId = itemDto.UnitId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice,
                    DiscountPercent = itemDto.DiscountPercent,
                    DiscountAmount = discountAmount,
                    TaxPercent = itemDto.TaxPercent,
                    TaxAmount = taxAmount,
                    TotalAmount = itemTotal,
                    Notes = itemDto.Notes
                });
            }

            var sale = new Domain.Entities.SalesInvoice
            {
                InvoiceNumber = request.InvoiceNumber,
                SaleDate = request.SaleDate,
                CustomerId = request.CustomerId,
                WarehouseId = request.WarehouseId,
                SubTotal = subTotal,
                TotalDiscount = totalDiscount,
                TotalTax = totalTax,
                TotalAmount = subTotal - totalDiscount + totalTax,
                PaidAmount = 0,
                PaymentStatus = (int)request.PaymentStatus,
                Status = (int)request.Status,
                DueDate = request.DueDate,
                Notes = request.Notes,
                SalesInvoiceItem = saleItems
            };

            await _saleRepository.AddAsync(sale);
            await _saleRepository.SaveChangesAsync();

            var saleDto = new SaleDto
            {
                Id = sale.Id,
                InvoiceNumber = sale.InvoiceNumber,
                SaleDate = sale.SaleDate,
                CustomerId = sale.CustomerId,
                WarehouseId = sale.WarehouseId,
                SubTotal = sale.SubTotal,
                TotalDiscount = sale.TotalDiscount,
                TotalTax = sale.TotalTax,
                TotalAmount = sale.TotalAmount,
                PaidAmount = sale.PaidAmount,
                PaymentStatus = (ERP.Domain.Enums.PaymentStatus)sale.PaymentStatus,
                Status = (ERP.Domain.Enums.SaleStatus)sale.Status,
                DueDate = sale.DueDate,
                Notes = sale.Notes,
                Items = sale.SalesInvoiceItem.Select(i => new SaleItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
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
