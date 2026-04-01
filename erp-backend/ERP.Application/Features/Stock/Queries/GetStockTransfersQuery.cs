using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Stock.Queries
{
    public class GetStockTransfersQuery : IRequest<Result<List<StockTransferDto>>>
    {
        public string? FromWarehouseId { get; set; }
        public string? ToWarehouseId { get; set; }
        public string? ProductId { get; set; }
        public int? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
