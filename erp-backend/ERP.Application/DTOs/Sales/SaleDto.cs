using ERP.Domain.Enums;

namespace ERP.Application.DTOs.Sales
{
    public class SaleDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public int CustomerId { get; set; }
        public string? WarehouseId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public SaleStatus Status { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Notes { get; set; }
        public string? CustomerName { get; set; }
        public List<SaleItemDto> Items { get; set; } = new();
    }
}
