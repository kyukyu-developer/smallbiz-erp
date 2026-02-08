using ERP.Shared.Contracts.Common;

namespace ERP.Shared.Contracts.Events;

public class SaleCreatedEvent : IntegrationEvent
{
    public int SaleId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public int? WarehouseId { get; set; }
    public decimal TotalAmount { get; set; }
    public List<SaleItemEvent> Items { get; set; } = new();
}

public class SaleItemEvent
{
    public int ProductId { get; set; }
    public int UnitId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
