#nullable disable
using System;
using System.Collections.Generic;

namespace ERP.Domain.Entities;

public partial class PurchOrderItem
{
    public string Id { get; set; }
    public string PurchaseOrderId { get; set; }
    public string ProductId { get; set; }
    public string UnitId { get; set; }
    public decimal Quantity { get; set; }
    public decimal ReceivedQuantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal? DiscountPercent { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? TaxPercent { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Notes { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public string LastAction { get; set; }

    public virtual PurchOrder PurchaseOrder { get; set; }
    public virtual ProdItem Product { get; set; }
    public virtual ProdUnit Unit { get; set; }
}
