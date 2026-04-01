namespace ERP.Application.DTOs.Stock
{
    public class StockMovementDto
    {
        public string Id { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string WarehouseId { get; set; } = string.Empty;
        public string WarehouseName { get; set; } = string.Empty;
        public string MovementType { get; set; } = string.Empty;
        public int ReferenceType { get; set; }
        public string ReferenceTypeName { get; set; } = string.Empty;
        public string ReferenceId { get; set; } = string.Empty;
        public decimal BaseQuantity { get; set; }
        public string? BatchId { get; set; }
        public string? BatchNo { get; set; }
        public string? SerialId { get; set; }
        public string? SerialNo { get; set; }
        public DateTime MovementDate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}
