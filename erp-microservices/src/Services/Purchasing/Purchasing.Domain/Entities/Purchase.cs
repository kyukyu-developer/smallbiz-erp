using Purchasing.Domain.Common;
using Purchasing.Domain.Enums;

namespace Purchasing.Domain.Entities;

public class Purchase : AuditableEntity
{
    public string PurchaseOrderNumber { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public int SupplierId { get; set; }
    public int? WarehouseId { get; set; }
    public decimal SubTotal { get; set; }
    public decimal? TotalDiscount { get; set; }
    public decimal? TotalTax { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal? PaidAmount { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;
    public PurchaseStatus Status { get; set; } = PurchaseStatus.Draft;
    public DateTime? ExpectedDate { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public string? Notes { get; set; }

    // Navigation
    public Supplier Supplier { get; set; } = null!;
    public ICollection<PurchaseItem> Items { get; set; } = new List<PurchaseItem>();
}
