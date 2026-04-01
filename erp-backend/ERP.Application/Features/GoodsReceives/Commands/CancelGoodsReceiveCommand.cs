using MediatR;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.GoodsReceives.Commands
{
    public class CancelGoodsReceiveCommand : IRequest<Result<bool>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
