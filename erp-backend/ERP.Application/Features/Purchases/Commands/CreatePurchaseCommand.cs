using MediatR;
using ERP.Application.DTOs.Purchases;
using ERP.Application.DTOs.Common;
using ERP.Domain.Enums;

namespace ERP.Application.Features.Purchases.Commands
{
    public class CreatePurchaseCommand : IRequest<Result<PurchaseDto>>
    {
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public int SupplierId { get; set; }
        public int? WarehouseId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PurchaseStatus Status { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public string? Notes { get; set; }
        public List<CreatePurchaseItemDto> Items { get; set; } = new();
    }
}
