using ERP.Domain.Common;
using ERP.Domain.Enums;

namespace ERP.Domain.Entities
{
    public class Sale : AuditableEntity
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public int CustomerId { get; set; }
        public string? WarehouseId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public SaleStatus Status { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Notes { get; set; }

        // Navigation properties
        public Customer Customer { get; set; } = null!;
        public Warehouse? Warehouse { get; set; }
        public ICollection<SalesItem> Items { get; set; } = new List<SalesItem>();
    }
}
