namespace ERP.Application.DTOs.Stock
{
    public class StockAdjustmentDto
    {
        public string Id { get; set; } = string.Empty;
        public string AdjustmentNo { get; set; } = string.Empty;
        public string WarehouseId { get; set; } = string.Empty;
        public string WarehouseName { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal AdjustmentQuantity { get; set; }
        public string? Reason { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}
