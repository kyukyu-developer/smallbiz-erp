using MediatR;
using ERP.Application.DTOs.Sales;
using ERP.Application.DTOs.Common;
using ERP.Domain.Enums;

namespace ERP.Application.Features.Sales.Queries
{
    public class GetSalesQuery : IRequest<Result<List<SaleDto>>>
    {
        public int? CustomerId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public SaleStatus? Status { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
    }
}
