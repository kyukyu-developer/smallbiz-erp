using MediatR;
using ERP.Application.DTOs.GoodsReceives;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.GoodsReceives.Commands
{
    public class CreateGoodsReceiveCommand : IRequest<Result<GoodsReceiveDto>>
    {
        public DateTime ReceiveDate { get; set; }
        public string? PurchaseOrderId { get; set; }
        public string SupplierId { get; set; } = string.Empty;
        public string WarehouseId { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public List<CreateGoodsReceiveItemDto> Items { get; set; } = new();
    }
}
