namespace ERP.Application.DTOs.PurchaseOrders
{
    public class CreatePurchaseOrderItemDto
    {
        public string ProductId { get; set; } = string.Empty;
        public string UnitId { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? TaxPercent { get; set; }
        public decimal? TaxAmount { get; set; }
        public string? Notes { get; set; }
    }
}
