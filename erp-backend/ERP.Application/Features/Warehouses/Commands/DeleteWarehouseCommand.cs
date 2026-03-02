using MediatR;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Warehouses.Commands
{
    public class DeleteWarehouseCommand : IRequest<Result<bool>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
