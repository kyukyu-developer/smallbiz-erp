#nullable disable
using System;
using System.Collections.Generic;

namespace ERP.Domain.Entities;

public partial class PurchGoodsReceive
{
    public string Id { get; set; }
    public string ReceiveNumber { get; set; }
    public DateTime ReceiveDate { get; set; }
    public string PurchaseOrderId { get; set; }
    public string SupplierId { get; set; }
    public string WarehouseId { get; set; }
    public int Status { get; set; }
    public string Notes { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public string LastAction { get; set; }

    public virtual PurchOrder PurchaseOrder { get; set; }
    public virtual PurchSupplier Supplier { get; set; }
    public virtual InvWarehouse Warehouse { get; set; }
    public virtual ICollection<PurchGoodsReceiveItem> PurchGoodsReceiveItem { get; set; } = new List<PurchGoodsReceiveItem>();
}
