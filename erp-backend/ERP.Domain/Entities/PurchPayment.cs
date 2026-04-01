#nullable disable
using System;
using System.Collections.Generic;

namespace ERP.Domain.Entities;

public partial class PurchPayment
{
    public string Id { get; set; }
    public string PaymentNumber { get; set; }
    public string PurchaseInvoiceId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public int PaymentMethod { get; set; }
    public string ReferenceNumber { get; set; }
    public string Notes { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public string LastAction { get; set; }

    public virtual PurchInvoice PurchaseInvoice { get; set; }
}
