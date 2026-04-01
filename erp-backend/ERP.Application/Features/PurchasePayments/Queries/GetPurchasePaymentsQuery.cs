using MediatR;
using ERP.Application.DTOs.PurchasePayments;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.PurchasePayments.Queries
{
    public class GetPurchasePaymentsQuery : IRequest<Result<List<PurchasePaymentDto>>>
    {
        public string PurchaseInvoiceId { get; set; } = string.Empty;
    }
}
