namespace ERP.Application.DTOs.Sales
{
    public class SaleItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UnitId { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? TaxPercent { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
        public string? ProductName { get; set; }
    }
}
