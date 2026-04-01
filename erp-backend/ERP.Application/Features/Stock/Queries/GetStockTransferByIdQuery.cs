using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Stock.Queries
{
    public class GetStockTransferByIdQuery : IRequest<Result<StockTransferDto>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
