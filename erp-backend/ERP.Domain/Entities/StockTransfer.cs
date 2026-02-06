using ERP.Domain.Common;
using ERP.Domain.Enums;

namespace ERP.Domain.Entities
{
    public class StockTransfer : AuditableEntity
    {
        public string TransferNo { get; set; } = string.Empty;
        public int FromWarehouseId { get; set; }
        public int ToWarehouseId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime TransferDate { get; set; }
        public EntityStatus Status { get; set; }
        public string? Notes { get; set; }

        // Navigation properties
        public Warehouse FromWarehouse { get; set; } = null!;
        public Warehouse ToWarehouse { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
