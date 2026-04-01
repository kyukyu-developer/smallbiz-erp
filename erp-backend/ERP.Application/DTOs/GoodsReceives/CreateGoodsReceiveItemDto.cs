namespace ERP.Application.DTOs.GoodsReceives
{
    public class CreateGoodsReceiveItemDto
    {
        public string? PurchaseOrderItemId { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public string UnitId { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public string? BatchId { get; set; }
        public string? SerialId { get; set; }
        public string? Notes { get; set; }
    }
}
