using ERP.Shared.Contracts.Common;

namespace ERP.Shared.Contracts.Events;

public class PurchaseReceivedEvent : IntegrationEvent
{
    public int PurchaseId { get; set; }
    public string PurchaseOrderNumber { get; set; } = string.Empty;
    public int SupplierId { get; set; }
    public int? WarehouseId { get; set; }
    public List<PurchaseItemEvent> Items { get; set; } = new();
}

public class PurchaseItemEvent
{
    public int ProductId { get; set; }
    public int UnitId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
}
