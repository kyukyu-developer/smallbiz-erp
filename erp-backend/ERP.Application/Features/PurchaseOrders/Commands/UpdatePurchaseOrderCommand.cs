using MediatR;
using ERP.Application.DTOs.PurchaseOrders;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.PurchaseOrders.Commands
{
    public class UpdatePurchaseOrderCommand : IRequest<Result<PurchaseOrderDto>>
    {
        public string Id { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string SupplierId { get; set; } = string.Empty;
        public string WarehouseId { get; set; } = string.Empty;
        public DateTime? ExpectedDate { get; set; }
        public string? Notes { get; set; }
        public List<CreatePurchaseOrderItemDto> Items { get; set; } = new();
    }
}
