using MediatR;
using ERP.Application.DTOs.Warehouses;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using ERP.Domain.Enums;

namespace ERP.Application.Features.Warehouses.Commands
{
    public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, Result<WarehouseDto>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateWarehouseCommandHandler(
            IWarehouseRepository warehouseRepository,
            IUnitOfWork unitOfWork)
        {
            _warehouseRepository = warehouseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<WarehouseDto>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
        {
            // Validate parent warehouse exists if provided
            if (!string.IsNullOrEmpty(request.ParentWarehouseId))
            {
                var parentExists = await _warehouseRepository.ExistsAsync(request.ParentWarehouseId);
                if (!parentExists)
                {
                    return Result<WarehouseDto>.Failure("Parent warehouse does not exist");
                }
            }

            // Check for duplicate warehouse name and city combination
            var existingWarehouse = await _warehouseRepository.GetByNameAndCityAsync(request.Name, request.City);
            if (existingWarehouse != null)
            {
                return Result<WarehouseDto>.Failure($"Warehouse with name '{request.Name}' already exists in {request.City ?? "this location"}");
            }

            var warehouse = new Warehouse
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                City = request.City,
                BranchType = request.BranchType,
                IsMainWarehouse = request.BranchType == BranchType.Main,
                ParentWarehouseId = request.ParentWarehouseId,
                IsUsedWarehouse = true,
                Active = true,
                Location = request.Location,
                Address = request.Address,
                Country = request.Country,
                ContactPerson = request.ContactPerson,
                Phone = request.Phone,
                LastAction = "CREATE",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System" // TODO: Get from current user context
            };

            await _warehouseRepository.AddAsync(warehouse);
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
