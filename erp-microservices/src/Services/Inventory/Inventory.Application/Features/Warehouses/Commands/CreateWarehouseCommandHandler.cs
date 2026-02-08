using ERP.Shared.Contracts.Common;
using Inventory.Application.DTOs.Warehouses;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Features.Warehouses.Commands;

public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, Result<WarehouseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateWarehouseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<WarehouseDto>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        // Check for duplicate code
        var existing = await _unitOfWork.Warehouses.FirstOrDefaultAsync(w => w.Code == request.Code);
        if (existing != null)
            return Result<WarehouseDto>.Failure($"A warehouse with code '{request.Code}' already exists.");

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
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Warehouses.AddAsync(warehouse);
        await _unitOfWork.SaveChangesAsync();

        var result = new WarehouseDto
        {
            Id = warehouse.Id,
            Code = warehouse.Code,
            Name = warehouse.Name,
            Location = warehouse.Location,
            Address = warehouse.Address,
            IsActive = warehouse.IsActive
        };

        return Result<WarehouseDto>.Success(result);
    }
}
