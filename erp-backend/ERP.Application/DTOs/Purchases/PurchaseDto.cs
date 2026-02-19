using ERP.Domain.Enums;

namespace ERP.Application.DTOs.Purchases
{
    public class PurchaseDto
    {
        public int Id { get; set; }
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public int SupplierId { get; set; }
        public string? WarehouseId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PurchaseStatus Status { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string? Notes { get; set; }
        public string? SupplierName { get; set; }
        public List<PurchaseItemDto> Items { get; set; } = new();
    }
}
