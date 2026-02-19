using MediatR;
using ERP.Application.DTOs.Warehouses;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Warehouses.Queries
{
    public class GetWarehouseByIdQuery : IRequest<Result<WarehouseDto>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
