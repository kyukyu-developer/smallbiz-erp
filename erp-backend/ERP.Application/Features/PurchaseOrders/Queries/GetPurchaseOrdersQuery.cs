using MediatR;
using ERP.Application.DTOs.PurchaseOrders;
using ERP.Application.DTOs.Common;
using ERP.Domain.Enums;

namespace ERP.Application.Features.PurchaseOrders.Queries
{
    public class GetPurchaseOrdersQuery : IRequest<Result<List<PurchaseOrderDto>>>
    {
        public string? SupplierId { get; set; }
        public PurchOrderStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
