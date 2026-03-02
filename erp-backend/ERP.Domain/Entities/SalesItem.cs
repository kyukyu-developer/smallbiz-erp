using ERP.Domain.Common;

namespace ERP.Domain.Entities
{
    public class SalesItem : BaseEntity
    {
        public string? SaleId { get; set; } 
        public string? ProductId { get; set; }
        public string? UnitId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? TaxPercent { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
        public string? BatchId { get; set; }
        public string? SerialId { get; set; }

        // Navigation properties
        public Sale Sale { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public Unit Unit { get; set; } = null!;
        public ProductBatch? Batch { get; set; }
        public ProductSerial? Serial { get; set; }
    }
}
