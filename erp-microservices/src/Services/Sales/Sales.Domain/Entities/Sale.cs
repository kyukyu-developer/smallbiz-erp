using Sales.Domain.Common;
using Sales.Domain.Enums;

namespace Sales.Domain.Entities;

public class Sale : AuditableEntity
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public int CustomerId { get; set; }
    public int? WarehouseId { get; set; }
    public decimal SubTotal { get; set; }
    public decimal? TotalDiscount { get; set; }
    public decimal? TotalTax { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal? PaidAmount { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;
    public SaleStatus Status { get; set; } = SaleStatus.Draft;
    public DateTime? DueDate { get; set; }
    public string? Notes { get; set; }

    // Navigation
    public Customer Customer { get; set; } = null!;
    public ICollection<SalesItem> Items { get; set; } = new List<SalesItem>();
}
