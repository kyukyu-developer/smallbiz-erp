using MediatR;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Purchases.Commands
{
    public class DeletePurchaseCommand : IRequest<Result<bool>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
