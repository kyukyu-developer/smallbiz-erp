using Inventory.Domain.Common;

namespace Inventory.Domain.Entities;

public class WarehouseStock : BaseEntity
{
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    public decimal Quantity { get; set; }

    // Navigation properties
    public Product Product { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
}
