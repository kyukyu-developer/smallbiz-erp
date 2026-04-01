using MediatR;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.PurchasePayments.Commands
{
    public class DeletePurchasePaymentCommand : IRequest<Result<bool>>
    {
        public string PurchaseInvoiceId { get; set; } = string.Empty;
        public string PaymentId { get; set; } = string.Empty;
    }
}
