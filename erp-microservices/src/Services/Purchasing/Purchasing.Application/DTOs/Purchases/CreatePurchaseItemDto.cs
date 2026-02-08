namespace Purchasing.Application.DTOs.Purchases;

public class CreatePurchaseItemDto
{
    public int ProductId { get; set; }
    public int UnitId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Tax { get; set; }
}
