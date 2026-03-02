using ERP.Domain.Common;
using ERP.Domain.Enums;

namespace ERP.Domain.Entities
{
    public class StockMovement : BaseEntity
    {
        public string? ProductId { get; set; }
        public string? WarehouseId { get; set; } 
        public MovementType MovementType { get; set; }
        public ReferenceType ReferenceType { get; set; }
        public string? ReferenceId { get; set; }
        public decimal BaseQuantity { get; set; }
        public string? BatchId { get; set; }
        public DateTime MovementDate { get; set; }
        public string? Notes { get; set; }

        // Navigation properties
        public Product Product { get; set; } = null!;
        public Warehouse Warehouse { get; set; } = null!;
        public ProductBatch? Batch { get; set; }
    }
}
