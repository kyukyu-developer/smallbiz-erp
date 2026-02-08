using ERP.Shared.Contracts.Common;
using Inventory.Application.DTOs.Stock;
using MediatR;

namespace Inventory.Application.Features.Stock.Queries;

public class GetWarehouseStockQuery : IRequest<Result<List<WarehouseStockDto>>>
{
    public int ProductId { get; set; }
}
