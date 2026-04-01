#nullable disable
using System;
using System.Collections.Generic;

namespace ERP.Domain.Entities;

public partial class PurchOrder
{
    public string Id { get; set; }
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public string SupplierId { get; set; }
    public string WarehouseId { get; set; }
    public decimal SubTotal { get; set; }
    public decimal? TotalDiscount { get; set; }
    public decimal? TotalTax { get; set; }
    public decimal TotalAmount { get; set; }
    public int Status { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public string Notes { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public string LastAction { get; set; }

    public virtual PurchSupplier Supplier { get; set; }
    public virtual InvWarehouse Warehouse { get; set; }
    public virtual ICollection<PurchOrderItem> PurchOrderItem { get; set; } = new List<PurchOrderItem>();
}
