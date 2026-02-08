namespace ERP.Shared.Contracts.DTOs;

public class ProductStockDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal AvailableQuantity { get; set; }
    public int WarehouseId { get; set; }
}
