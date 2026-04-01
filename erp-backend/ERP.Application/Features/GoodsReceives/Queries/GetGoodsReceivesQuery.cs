using MediatR;
using ERP.Application.DTOs.GoodsReceives;
using ERP.Application.DTOs.Common;
using ERP.Domain.Enums;

namespace ERP.Application.Features.GoodsReceives.Queries
{
    public class GetGoodsReceivesQuery : IRequest<Result<List<GoodsReceiveDto>>>
    {
        public string? SupplierId { get; set; }
        public string? WarehouseId { get; set; }
        public GoodsReceiveStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
