using MediatR;
using ERP.Application.DTOs.Warehouses;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;
using ERP.Domain.Enums;

namespace ERP.Application.Features.Warehouses.Commands
{
    public class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommand, Result<WarehouseDto>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateWarehouseCommandHandler(
            IWarehouseRepository warehouseRepository,
            IUnitOfWork unitOfWork)
        {
            _warehouseRepository = warehouseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<WarehouseDto>> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
        {
            // Check if warehouse exists
            var warehouse = await _warehouseRepository.GetByIdAsync(request.Id);
            if (warehouse == null)
            {
                return Result<WarehouseDto>.Failure($"Warehouse with ID '{request.Id}' not found");
            }

            // Validate parent warehouse if provided
            if (!string.IsNullOrEmpty(request.ParentWarehouseId))
            {
                // Prevent circular reference first — no DB call needed
                if (request.ParentWarehouseId == request.Id)
                {
                    return Result<WarehouseDto>.Failure("Warehouse cannot be its own parent");
                }

                var parentExists = await _warehouseRepository.ExistsAsync(request.ParentWarehouseId);
                if (!parentExists)
                {
                    return Result<WarehouseDto>.Failure("Parent warehouse does not exist");
                }
            }

            // Check for duplicate warehouse name and city combination (excluding current warehouse)
            var existingWarehouse = await _warehouseRepository.GetByNameAndCityAsync(request.Name, request.City);
            if (existingWarehouse != null && existingWarehouse.Id != request.Id)
            {
                return Result<WarehouseDto>.Failure($"Warehouse with name '{request.Name}' already exists in {request.City ?? "this location"}");
            }

            // Update warehouse
            warehouse.Name = request.Name;
            warehouse.City = request.City;
            warehouse.BranchType = request.BranchType;
            warehouse.IsMainWarehouse = request.BranchType == BranchType.Main;
            warehouse.ParentWarehouseId = request.ParentWarehouseId;
            warehouse.Active = request.Active;
            warehouse.Location = request.Location;
            warehouse.Address = request.Address;
            warehouse.Country = request.Country;
            warehouse.ContactPerson = request.ContactPerson;
            warehouse.Phone = request.Phone;
            warehouse.LastAction = "UPDATE";
            warehouse.UpdatedAt = DateTime.UtcNow;
            warehouse.UpdatedBy = "System"; // TODO: Get from current user context

            _warehouseRepository.Update(warehouse);
            await _unitOfWork.SaveChangesAsync();

            // Get parent warehouse name if applicable
            string? parentWarehouseName = null;
            if (!string.IsNullOrEmpty(request.ParentWarehouseId))
            {
                var parentWarehouse = await _warehouseRepository.GetByIdAsync(request.ParentWarehouseId);
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
