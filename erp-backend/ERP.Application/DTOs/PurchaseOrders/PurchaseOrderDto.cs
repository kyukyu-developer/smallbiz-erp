using ERP.Domain.Enums;

namespace ERP.Application.DTOs.PurchaseOrders
{
    public class PurchaseOrderDto
    {
        public string Id { get; set; } = string.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string? WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
        public decimal SubTotal { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal TotalAmount { get; set; }
        public PurchOrderStatus Status { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public List<PurchaseOrderItemDto> Items { get; set; } = new();
    }
}
