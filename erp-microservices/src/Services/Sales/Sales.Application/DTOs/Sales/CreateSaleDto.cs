namespace Sales.Application.DTOs.Sales;

public class CreateSaleDto
{
    public int CustomerId { get; set; }
    public int? WarehouseId { get; set; }
    public DateTime SaleDate { get; set; }
    public string? Notes { get; set; }
    public List<CreateSaleItemDto> Items { get; set; } = new();
}
