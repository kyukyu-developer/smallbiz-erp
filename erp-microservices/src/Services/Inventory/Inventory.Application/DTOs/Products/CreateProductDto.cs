namespace Inventory.Application.DTOs.Products;

public class CreateProductDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public int BaseUnitId { get; set; }
    public decimal? MinimumStock { get; set; }
    public decimal? MaximumStock { get; set; }
    public decimal? ReorderLevel { get; set; }
}
