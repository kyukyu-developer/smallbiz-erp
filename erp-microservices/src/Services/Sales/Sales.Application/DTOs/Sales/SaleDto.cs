namespace Sales.Application.DTOs.Sales;

public class SaleDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int? WarehouseId { get; set; }
    public decimal SubTotal { get; set; }
    public decimal? TotalDiscount { get; set; }
    public decimal? TotalTax { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal? PaidAmount { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<SaleItemDto> Items { get; set; } = new();
}
