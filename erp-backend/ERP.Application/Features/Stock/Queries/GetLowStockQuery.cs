using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Stock.Queries
{
    public class GetLowStockQuery : IRequest<Result<List<StockLevelDto>>>
    {
        public string? WarehouseId { get; set; }
    }
}
