using MediatR;
using ERP.Application.DTOs.Sales;
using ERP.Application.DTOs.Common;
using ERP.Domain.Enums;

namespace ERP.Application.Features.Sales.Commands
{
    public class CreateSaleCommand : IRequest<Result<SaleDto>>
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public int CustomerId { get; set; }
        public string? WarehouseId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public SaleStatus Status { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Notes { get; set; }
        public List<CreateSaleItemDto> Items { get; set; } = new();
    }
}
