using MediatR;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.PurchaseOrders.Commands
{
    public class CancelPurchaseOrderCommand : IRequest<Result<bool>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
