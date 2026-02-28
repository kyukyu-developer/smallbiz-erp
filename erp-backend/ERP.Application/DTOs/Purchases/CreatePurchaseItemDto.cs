namespace ERP.Application.DTOs.Purchases
{
    public class CreatePurchaseItemDto
    {
        public int ProductId { get; set; }
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
