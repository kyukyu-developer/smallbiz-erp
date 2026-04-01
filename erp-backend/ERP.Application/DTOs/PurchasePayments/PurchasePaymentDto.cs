using ERP.Domain.Enums;

namespace ERP.Application.DTOs.PurchasePayments
{
    public class PurchasePaymentDto
    {
        public string Id { get; set; } = string.Empty;
        public string PaymentNumber { get; set; } = string.Empty;
        public string PurchaseInvoiceId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}
