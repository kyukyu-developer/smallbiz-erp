using ERP.Shared.Contracts.Common;

namespace ERP.Shared.Contracts.Events;

public class SaleCancelledEvent : IntegrationEvent
{
    public int SaleId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int? WarehouseId { get; set; }
    public List<SaleItemEvent> Items { get; set; } = new();
}
