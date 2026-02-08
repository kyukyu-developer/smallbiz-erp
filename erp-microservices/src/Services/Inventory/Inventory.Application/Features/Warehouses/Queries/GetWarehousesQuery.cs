using ERP.Shared.Contracts.Common;
using Inventory.Application.DTOs.Warehouses;
using MediatR;

namespace Inventory.Application.Features.Warehouses.Queries;

public class GetWarehousesQuery : IRequest<Result<List<WarehouseDto>>>
{
}
