namespace Inventory.Application.DTOs.Warehouses;

public class WarehouseDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
}
