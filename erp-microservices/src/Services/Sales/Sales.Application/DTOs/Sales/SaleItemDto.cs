namespace Sales.Application.DTOs.Sales;

public class SaleItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int UnitId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Tax { get; set; }
    public decimal TotalPrice { get; set; }
}
