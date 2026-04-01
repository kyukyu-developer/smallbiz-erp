using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Stock.Queries
{
    public class GetStockAdjustmentsQuery : IRequest<Result<List<StockAdjustmentDto>>>
    {
        public string? WarehouseId { get; set; }
        public string? ProductId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
