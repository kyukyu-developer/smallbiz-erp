using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Stock.Queries
{
    public class GetStockLevelsQuery : IRequest<Result<List<StockLevelDto>>>
    {
        public string? WarehouseId { get; set; }
        public string? ProductId { get; set; }
        public bool? LowStockOnly { get; set; }
    }
}
