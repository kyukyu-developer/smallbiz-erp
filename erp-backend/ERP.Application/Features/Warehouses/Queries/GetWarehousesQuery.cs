using MediatR;
using ERP.Application.DTOs.Warehouses;
using ERP.Application.DTOs.Common;
using ERP.Domain.Enums;

namespace ERP.Application.Features.Warehouses.Queries
{
    public class GetWarehousesQuery : IRequest<Result<List<WarehouseDto>>>
    {
        public bool? IncludeInactive { get; set; }
        public BranchType? BranchType { get; set; }
        public bool? MainWarehousesOnly { get; set; }
    }
}
