using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Stock.Commands
{
    public class CreateStockAdjustmentCommand : IRequest<Result<StockAdjustmentDto>>
    {
        public string WarehouseId { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public decimal AdjustmentQuantity { get; set; }
        public string? Reason { get; set; }
        public DateTime AdjustmentDate { get; set; }
    }
}
