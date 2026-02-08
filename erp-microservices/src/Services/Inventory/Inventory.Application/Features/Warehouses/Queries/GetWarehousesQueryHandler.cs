using ERP.Shared.Contracts.Common;
using Inventory.Application.DTOs.Warehouses;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Features.Warehouses.Queries;

public class GetWarehousesQueryHandler : IRequestHandler<GetWarehousesQuery, Result<List<WarehouseDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetWarehousesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<WarehouseDto>>> Handle(GetWarehousesQuery request, CancellationToken cancellationToken)
    {
        var warehouses = await _unitOfWork.Warehouses.GetAllAsync();

        var result = warehouses.Select(w => new WarehouseDto
        {
            Id = w.Id,
            Code = w.Code,
            Name = w.Name,
            Location = w.Location,
            Address = w.Address,
            IsActive = w.IsActive
        }).ToList();

        return Result<List<WarehouseDto>>.Success(result);
    }
}
