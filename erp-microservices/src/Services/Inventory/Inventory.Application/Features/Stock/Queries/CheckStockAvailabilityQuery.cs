using ERP.Shared.Contracts.Common;
using MediatR;

namespace Inventory.Application.Features.Stock.Queries;

public class CheckStockAvailabilityQuery : IRequest<Result<bool>>
{
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    public decimal RequiredQuantity { get; set; }
}
