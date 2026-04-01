namespace ERP.Application.DTOs.PurchaseOrders
{
    public class PurchaseOrderItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public string UnitId { get; set; } = string.Empty;
        public string? UnitName { get; set; }
        public decimal Quantity { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? TaxPercent { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
    }
}
