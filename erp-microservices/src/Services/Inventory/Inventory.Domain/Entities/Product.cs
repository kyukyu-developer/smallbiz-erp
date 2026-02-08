using Inventory.Domain.Common;

namespace Inventory.Domain.Entities;

public class Product : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public int BaseUnitId { get; set; }
    public decimal? MinimumStock { get; set; }
    public decimal? MaximumStock { get; set; }
    public decimal? ReorderLevel { get; set; }
    public string? Barcode { get; set; }
    public bool IsBatchTracked { get; set; }
    public bool IsSerialTracked { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Category Category { get; set; } = null!;
    public Unit BaseUnit { get; set; } = null!;
    public ICollection<ProductUnitPrice> ProductUnitPrices { get; set; } = new List<ProductUnitPrice>();
    public ICollection<WarehouseStock> WarehouseStocks { get; set; } = new List<WarehouseStock>();
}
