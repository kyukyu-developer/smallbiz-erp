using MediatR;
using ERP.Application.DTOs.PurchaseOrders;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.PurchaseOrders.Commands
{
    public class ApprovePurchaseOrderCommand : IRequest<Result<PurchaseOrderDto>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
