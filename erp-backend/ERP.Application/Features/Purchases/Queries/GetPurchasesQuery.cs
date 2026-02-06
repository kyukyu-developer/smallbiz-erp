using MediatR;
using ERP.Application.DTOs.Purchases;
using ERP.Application.DTOs.Common;
using ERP.Domain.Enums;

namespace ERP.Application.Features.Purchases.Queries
{
    public class GetPurchasesQuery : IRequest<Result<List<PurchaseDto>>>
    {
        public int? SupplierId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PurchaseStatus? Status { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
    }
}
