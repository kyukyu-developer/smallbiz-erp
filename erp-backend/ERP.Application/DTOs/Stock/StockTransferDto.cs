namespace ERP.Application.DTOs.Stock
{
    public class StockTransferDto
    {
        public string Id { get; set; } = string.Empty;
        public string TransferNo { get; set; } = string.Empty;
        public string FromWarehouseId { get; set; } = string.Empty;
        public string FromWarehouseName { get; set; } = string.Empty;
        public string ToWarehouseId { get; set; } = string.Empty;
        public string ToWarehouseName { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public DateTime TransferDate { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}
