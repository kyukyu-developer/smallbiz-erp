using MediatR;
using ERP.Application.DTOs.Warehouses;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Warehouses.Queries
{
    public class GetWarehousesQueryHandler : IRequestHandler<GetWarehousesQuery, Result<List<WarehouseDto>>>
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public GetWarehousesQueryHandler(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<Result<List<WarehouseDto>>> Handle(GetWarehousesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Domain.Entities.Warehouse> warehouses;

            // Filter by branch type if specified
            if (request.MainWarehousesOnly == true)
            {
                warehouses = await _warehouseRepository.GetMainWarehousesAsync();
            }
            else if (request.BranchType.HasValue)
            {
                warehouses = await _warehouseRepository.GetByBranchTypeAsync(request.BranchType.Value);
            }
            else
            {
                warehouses = await _warehouseRepository.GetAllAsync();
            }

            // Apply active filter
            var filteredWarehouses = warehouses
                .Where(w => (request.IncludeInactive ?? false) || w.Active)
                .Select(w => new WarehouseDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    City = w.City,
                    BranchType = w.BranchType.ToString(),
                    IsMainWarehouse = w.IsMainWarehouse,
                    ParentWarehouseId = w.ParentWarehouseId,
                    ParentWarehouseName = w.ParentWarehouse?.Name,
                    IsUsedWarehouse = w.IsUsedWarehouse,
                    Active = w.Active,
                    Location = w.Location,
                    Address = w.Address,
                    Country = w.Country,
                    ContactPerson = w.ContactPerson,
                    Phone = w.Phone,
                    CreatedOn = w.CreatedAt,
                    ModifiedOn = w.UpdatedAt,
                    CreatedBy = w.CreatedBy,
                    ModifiedBy = w.UpdatedBy,
                    LastAction = w.LastAction
                })
                .ToList();

            return Result<List<WarehouseDto>>.Success(filteredWarehouses);
        }
    }
}
