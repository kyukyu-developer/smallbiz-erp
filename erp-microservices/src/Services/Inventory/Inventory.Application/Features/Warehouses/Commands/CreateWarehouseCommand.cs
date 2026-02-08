using ERP.Shared.Contracts.Common;
using Inventory.Application.DTOs.Warehouses;
using MediatR;

namespace Inventory.Application.Features.Warehouses.Commands;

public class CreateWarehouseCommand : IRequest<Result<WarehouseDto>>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? ContactPerson { get; set; }
    public string? Phone { get; set; }
}
