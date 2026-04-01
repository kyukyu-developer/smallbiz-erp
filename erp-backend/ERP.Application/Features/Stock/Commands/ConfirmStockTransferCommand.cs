using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Stock.Commands
{
    public class ConfirmStockTransferCommand : IRequest<Result<StockTransferDto>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
