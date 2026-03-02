using ERP.Domain.Common;

namespace ERP.Domain.Entities
{
    public class Product : AuditableEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CategoryId { get; set; }
        public string? BaseUnitId { get; set; } 
        public decimal? MinimumStock { get; set; }
        public decimal? MaximumStock { get; set; }
        public decimal? ReorderLevel { get; set; }
        public string? Barcode { get; set; }
        public bool IsBatchTracked { get; set; }
        public bool IsSerialTracked { get; set; }

        // Navigation properties
        public Category Category { get; set; } = null!;
        public Unit BaseUnit { get; set; } = null!;
        public ICollection<ProductUnitPrice> UnitPrices { get; set; } = new List<ProductUnitPrice>();
        public ICollection<UnitConversion> UnitConversions { get; set; } = new List<UnitConversion>();
        public ICollection<WarehouseStock> WarehouseStocks { get; set; } = new List<WarehouseStock>();
        public ICollection<ProductBatch> Batches { get; set; } = new List<ProductBatch>();
        public ICollection<ProductSerial> Serials { get; set; } = new List<ProductSerial>();
        public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    }
}
