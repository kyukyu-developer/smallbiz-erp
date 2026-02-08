using Purchasing.Domain.Common;

namespace Purchasing.Domain.Entities;

public class PurchaseItem : BaseEntity
{
    public int PurchaseId { get; set; }
    public int ProductId { get; set; }
    public int UnitId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Tax { get; set; }
    public decimal TotalCost { get; set; }

    // Navigation
    public Purchase Purchase { get; set; } = null!;
}
