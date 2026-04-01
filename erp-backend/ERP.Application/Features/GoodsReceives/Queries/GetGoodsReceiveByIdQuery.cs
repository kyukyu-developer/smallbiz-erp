using MediatR;
using ERP.Application.DTOs.GoodsReceives;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.GoodsReceives.Queries
{
    public class GetGoodsReceiveByIdQuery : IRequest<Result<GoodsReceiveDto>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
