using MediatR;
using ERP.Application.DTOs.Warehouses;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Warehouses.Queries
{
    public class GetWarehousesQuery : IRequest<Result<List<WarehouseDto>>>
    {
        public bool? IncludeInactive { get; set; }
    }
}
