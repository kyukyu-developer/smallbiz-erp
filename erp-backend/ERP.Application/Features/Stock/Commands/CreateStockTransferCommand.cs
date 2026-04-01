using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Stock.Commands
{
    public class CreateStockTransferCommand : IRequest<Result<StockTransferDto>>
    {
        public string FromWarehouseId { get; set; } = string.Empty;
        public string ToWarehouseId { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public DateTime TransferDate { get; set; }
        public string? Notes { get; set; }
    }
}
