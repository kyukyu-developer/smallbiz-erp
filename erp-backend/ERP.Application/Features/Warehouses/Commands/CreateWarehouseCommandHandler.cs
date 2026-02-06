using MediatR;
using ERP.Application.DTOs.Warehouses;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Warehouses.Commands
{
    public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, Result<WarehouseDto>>
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public CreateWarehouseCommandHandler(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<Result<WarehouseDto>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouse = new Warehouse
            {
                Code = request.Code,
                Name = request.Name,
                Location = request.Location,
                Address = request.Address,
                City = request.City,
                Country = request.Country,
                ContactPerson = request.ContactPerson,
                Phone = request.Phone,
                IsActive = request.IsActive
            };

            await _warehouseRepository.AddAsync(warehouse);
            await _warehouseRepository.SaveChangesAsync();

            var warehouseDto = new WarehouseDto
            {
                Id = warehouse.Id,
                Code = warehouse.Code,
                Name = warehouse.Name,
                Location = warehouse.Location,
                Address = warehouse.Address,
                City = warehouse.City,
                Country = warehouse.Country,
                ContactPerson = warehouse.ContactPerson,
                Phone = warehouse.Phone,
                IsActive = warehouse.IsActive
            };

            return Result<WarehouseDto>.Success(warehouseDto);
        }
    }
}
