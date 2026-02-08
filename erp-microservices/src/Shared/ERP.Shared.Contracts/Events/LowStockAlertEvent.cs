using ERP.Shared.Contracts.Common;

namespace ERP.Shared.Contracts.Events;

public class LowStockAlertEvent : IntegrationEvent
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int WarehouseId { get; set; }
    public decimal CurrentStock { get; set; }
    public decimal ReorderLevel { get; set; }
}
