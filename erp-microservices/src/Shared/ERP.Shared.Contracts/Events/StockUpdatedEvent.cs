using ERP.Shared.Contracts.Common;

namespace ERP.Shared.Contracts.Events;

public class StockUpdatedEvent : IntegrationEvent
{
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    public decimal NewQuantity { get; set; }
    public string Reason { get; set; } = string.Empty;
}
