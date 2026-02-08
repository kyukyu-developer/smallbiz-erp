using ERP.Shared.Contracts.Common;
using Inventory.Application.DTOs.Stock;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Features.Stock.Queries;

public class GetWarehouseStockQueryHandler : IRequestHandler<GetWarehouseStockQuery, Result<List<WarehouseStockDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetWarehouseStockQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<WarehouseStockDto>>> Handle(GetWarehouseStockQuery request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
        if (product == null)
            return Result<List<WarehouseStockDto>>.Failure("Product not found.");

        var stocks = await _unitOfWork.WarehouseStocks.FindAsync(s => s.ProductId == request.ProductId);
        var warehouses = (await _unitOfWork.Warehouses.GetAllAsync()).ToDictionary(w => w.Id, w => w.Name);

        var result = stocks.Select(s => new WarehouseStockDto
        {
            ProductId = s.ProductId,
            ProductName = product.Name,
            WarehouseId = s.WarehouseId,
            WarehouseName = warehouses.GetValueOrDefault(s.WarehouseId, string.Empty),
            Quantity = s.Quantity
        }).ToList();

        return Result<List<WarehouseStockDto>>.Success(result);
    }
}
