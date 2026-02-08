namespace Purchasing.Application.DTOs.Purchases;

public class CreatePurchaseDto
{
    public int SupplierId { get; set; }
    public int? WarehouseId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public string? Notes { get; set; }
    public List<CreatePurchaseItemDto> Items { get; set; } = new();
}
