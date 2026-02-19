using MediatR;
using ERP.Application.DTOs.Warehouses;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Warehouses.Queries
{
    public class GetWarehouseByIdQueryHandler : IRequestHandler<GetWarehouseByIdQuery, Result<WarehouseDto>>
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public GetWarehouseByIdQueryHandler(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<Result<WarehouseDto>> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
        {
            var warehouse = await _warehouseRepository.GetByIdAsync(request.Id);

            if (warehouse == null)
            {
                return Result<WarehouseDto>.Failure($"Warehouse with ID '{request.Id}' not found");
            }

            // Get parent warehouse name if applicable
            string? parentWarehouseName = null;
            if (!string.IsNullOrEmpty(warehouse.ParentWarehouseId))
            {
                var parentWarehouse = await _warehouseRepository.GetByIdAsync(warehouse.ParentWarehouseId);
                parentWarehouseName = parentWarehouse?.Name;
            }

            var warehouseDto = new WarehouseDto
            {
                Id = warehouse.Id,
                Name = warehouse.Name,
                City = warehouse.City,
                BranchType = warehouse.BranchType.ToString(),
                IsMainWarehouse = warehouse.IsMainWarehouse,
                ParentWarehouseId = warehouse.ParentWarehouseId,
                ParentWarehouseName = parentWarehouseName,
                IsUsedWarehouse = warehouse.IsUsedWarehouse,
                Active = warehouse.Active,
                Location = warehouse.Location,
                Address = warehouse.Address,
                Country = warehouse.Country,
                ContactPerson = warehouse.ContactPerson,
                Phone = warehouse.Phone,
                CreatedOn = warehouse.CreatedAt,
                ModifiedOn = warehouse.UpdatedAt,
                CreatedBy = warehouse.CreatedBy,
                ModifiedBy = warehouse.UpdatedBy,
                LastAction = warehouse.LastAction
            };

            return Result<WarehouseDto>.Success(warehouseDto);
        }
    }
}
