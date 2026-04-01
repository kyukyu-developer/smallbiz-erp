using ERP.Domain.Enums;

namespace ERP.Application.DTOs.GoodsReceives
{
    public class GoodsReceiveDto
    {
        public string Id { get; set; } = string.Empty;
        public string ReceiveNumber { get; set; } = string.Empty;
        public DateTime ReceiveDate { get; set; }
        public string? PurchaseOrderId { get; set; }
        public string? PurchaseOrderNumber { get; set; }
        public string SupplierId { get; set; } = string.Empty;
        public string? SupplierName { get; set; }
        public string WarehouseId { get; set; } = string.Empty;
        public string? WarehouseName { get; set; }
        public GoodsReceiveStatus Status { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public List<GoodsReceiveItemDto> Items { get; set; } = new();
    }
}
