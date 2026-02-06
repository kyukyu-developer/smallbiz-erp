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
            var warehouses = await _warehouseRepository.GetAllAsync();

            var filteredWarehouses = warehouses
                .Where(w => (request.IncludeInactive ?? false) || w.IsActive)
                .Select(w => new WarehouseDto
                {
                    Id = w.Id,
                    Code = w.Code,
                    Name = w.Name,
                    Location = w.Location,
                    Address = w.Address,
                    City = w.City,
                    Country = w.Country,
                    ContactPerson = w.ContactPerson,
                    Phone = w.Phone,
                    IsActive = w.IsActive
                })
                .ToList();

            return Result<List<WarehouseDto>>.Success(filteredWarehouses);
        }
    }
}
