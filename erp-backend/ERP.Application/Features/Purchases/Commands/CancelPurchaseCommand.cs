using MediatR;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Purchases.Commands
{
    public class CancelPurchaseCommand : IRequest<Result<bool>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
