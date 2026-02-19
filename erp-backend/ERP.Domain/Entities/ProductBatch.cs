using ERP.Domain.Common;

namespace ERP.Domain.Entities
{
    public class ProductBatch : BaseEntity
    {
        public int ProductId { get; set; }
        public string WarehouseId { get; set; } = string.Empty;
        public string BatchNo { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        // Navigation properties
        public Product Product { get; set; } = null!;
        public Warehouse Warehouse { get; set; } = null!;
        public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    }
}
