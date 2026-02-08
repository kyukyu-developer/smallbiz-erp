using Purchasing.Domain.Enums;

namespace Purchasing.Application.DTOs.Purchases;

public class PurchaseDto
{
    public int Id { get; set; }
    public string PurchaseOrderNumber { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public int? WarehouseId { get; set; }
    public decimal SubTotal { get; set; }
    public decimal? TotalDiscount { get; set; }
    public decimal? TotalTax { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal? PaidAmount { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public PurchaseStatus Status { get; set; }
    public List<PurchaseItemDto> Items { get; set; } = new();
}
