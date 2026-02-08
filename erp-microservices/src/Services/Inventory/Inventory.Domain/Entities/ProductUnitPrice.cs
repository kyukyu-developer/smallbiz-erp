using Inventory.Domain.Common;

namespace Inventory.Domain.Entities;

public class ProductUnitPrice : BaseEntity
{
    public int ProductId { get; set; }
    public int UnitId { get; set; }
    public decimal ConversionFactor { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }

    // Navigation properties
    public Product Product { get; set; } = null!;
    public Unit Unit { get; set; } = null!;
}
