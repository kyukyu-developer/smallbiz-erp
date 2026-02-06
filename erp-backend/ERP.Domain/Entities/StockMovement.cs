using ERP.Domain.Common;
using ERP.Domain.Enums;

namespace ERP.Domain.Entities
{
    public class StockMovement : BaseEntity
    {
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public MovementType MovementType { get; set; }
        public ReferenceType ReferenceType { get; set; }
        public int ReferenceId { get; set; }
        public decimal BaseQuantity { get; set; }
        public int? BatchId { get; set; }
        public DateTime MovementDate { get; set; }
        public string? Notes { get; set; }

        // Navigation properties
        public Product Product { get; set; } = null!;
        public Warehouse Warehouse { get; set; } = null!;
        public ProductBatch? Batch { get; set; }
    }
}
