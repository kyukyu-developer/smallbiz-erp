using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Stock.Queries
{
    public class GetStockAdjustmentByIdQuery : IRequest<Result<StockAdjustmentDto>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
