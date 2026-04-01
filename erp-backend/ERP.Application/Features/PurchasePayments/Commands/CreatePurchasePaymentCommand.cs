using MediatR;
using ERP.Application.DTOs.PurchasePayments;
using ERP.Application.DTOs.Common;
using ERP.Domain.Enums;

namespace ERP.Application.Features.PurchasePayments.Commands
{
    public class CreatePurchasePaymentCommand : IRequest<Result<PurchasePaymentDto>>
    {
        public string PurchaseInvoiceId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
    }
}
