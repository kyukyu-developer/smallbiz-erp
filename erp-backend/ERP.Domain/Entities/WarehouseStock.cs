using ERP.Domain.Common;

namespace ERP.Domain.Entities
{
    public class WarehouseStock : BaseEntity
    {
        public string WarehouseId { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal ReservedQuantity { get; set; }

        // Navigation properties
        public Warehouse Warehouse { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
