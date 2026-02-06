using ERP.Domain.Common;

namespace ERP.Domain.Entities
{
    public class SalesItem : BaseEntity
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? TaxPercent { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
        public int? BatchId { get; set; }
        public int? SerialId { get; set; }

        // Navigation properties
        public Sale Sale { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public Unit Unit { get; set; } = null!;
        public ProductBatch? Batch { get; set; }
        public ProductSerial? Serial { get; set; }
    }
}
