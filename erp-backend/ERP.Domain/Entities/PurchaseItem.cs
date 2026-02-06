using ERP.Domain.Common;

namespace ERP.Domain.Entities
{
    public class PurchaseItem : BaseEntity
    {
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? TaxPercent { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
        public int? BatchId { get; set; }

        // Navigation properties
        public Purchase Purchase { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public Unit Unit { get; set; } = null!;
        public ProductBatch? Batch { get; set; }
    }
}
