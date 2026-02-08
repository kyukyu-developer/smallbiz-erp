using Sales.Domain.Common;

namespace Sales.Domain.Entities;

public class SalesItem : BaseEntity
{
    public int SaleId { get; set; }
    public int ProductId { get; set; }
    public int UnitId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Tax { get; set; }
    public decimal TotalPrice { get; set; }

    // Navigation
    public Sale Sale { get; set; } = null!;
}
