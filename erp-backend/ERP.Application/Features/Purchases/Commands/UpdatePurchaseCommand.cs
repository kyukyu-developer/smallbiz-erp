using MediatR;
using ERP.Application.DTOs.Purchases;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Purchases.Commands
{
    public class UpdatePurchaseCommand : IRequest<Result<PurchaseDto>>
    {
        public string Id { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public string? SupplierId { get; set; }
        public string? WarehouseId { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public string? Notes { get; set; }
        public List<CreatePurchaseItemDto> Items { get; set; } = new();
    }
}
