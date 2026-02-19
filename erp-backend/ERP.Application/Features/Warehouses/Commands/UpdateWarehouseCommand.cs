using MediatR;
using ERP.Application.DTOs.Warehouses;
using ERP.Application.DTOs.Common;
using ERP.Domain.Enums;

namespace ERP.Application.Features.Warehouses.Commands
{
    public class UpdateWarehouseCommand : IRequest<Result<WarehouseDto>>
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? City { get; set; }
        public BranchType BranchType { get; set; }
        public string? ParentWarehouseId { get; set; }
        public bool Active { get; set; } = true;
        public string? Location { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
    }
}
