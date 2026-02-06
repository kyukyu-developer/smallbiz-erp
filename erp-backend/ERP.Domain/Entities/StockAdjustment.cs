using ERP.Domain.Common;

namespace ERP.Domain.Entities
{
    public class StockAdjustment : AuditableEntity
    {
        public string AdjustmentNo { get; set; } = string.Empty;
        public int WarehouseId { get; set; }
        public int ProductId { get; set; }
        public decimal AdjustmentQuantity { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime AdjustmentDate { get; set; }

        // Navigation properties
        public Warehouse Warehouse { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
