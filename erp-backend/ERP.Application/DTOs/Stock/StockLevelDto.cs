namespace ERP.Application.DTOs.Stock
{
    public class StockLevelDto
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string WarehouseId { get; set; } = string.Empty;
        public string WarehouseName { get; set; } = string.Empty;
        public decimal AvailableQuantity { get; set; }
        public decimal ReservedQuantity { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal? MinimumStock { get; set; }
        public decimal? ReorderLevel { get; set; }
        public bool IsBelowMinimum { get; set; }
        public bool NeedsReorder { get; set; }
    }
}
