namespace Sales.Application.DTOs.Sales;

public class CreateSaleItemDto
{
    public int ProductId { get; set; }
    public int UnitId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Tax { get; set; }
}
