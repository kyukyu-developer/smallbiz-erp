namespace ERP.Application.DTOs.GoodsReceives
{
    public class GoodsReceiveItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string? PurchaseOrderItemId { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public string UnitId { get; set; } = string.Empty;
        public string? UnitName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public string? BatchId { get; set; }
        public string? SerialId { get; set; }
        public string? Notes { get; set; }
    }
}
