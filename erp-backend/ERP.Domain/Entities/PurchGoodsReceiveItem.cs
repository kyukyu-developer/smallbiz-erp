#nullable disable
using System;
using System.Collections.Generic;

namespace ERP.Domain.Entities;

public partial class PurchGoodsReceiveItem
{
    public string Id { get; set; }
    public string GoodsReceiveId { get; set; }
    public string PurchaseOrderItemId { get; set; }
    public string ProductId { get; set; }
    public string UnitId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public string BatchId { get; set; }
    public string SerialId { get; set; }
    public string Notes { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public string LastAction { get; set; }

    public virtual PurchGoodsReceive GoodsReceive { get; set; }
    public virtual PurchOrderItem PurchaseOrderItem { get; set; }
    public virtual ProdItem Product { get; set; }
    public virtual ProdUnit Unit { get; set; }
    public virtual ProdBatch Batch { get; set; }
    public virtual ProdSerial Serial { get; set; }
}
