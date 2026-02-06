using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Stock.Queries
{
    public class GetStockLevelsQueryHandler : IRequestHandler<GetStockLevelsQuery, Result<List<StockLevelDto>>>
    {
        private readonly IWarehouseStockRepository _warehouseStockRepository;

        public GetStockLevelsQueryHandler(IWarehouseStockRepository warehouseStockRepository)
        {
            _warehouseStockRepository = warehouseStockRepository;
        }

        public async Task<Result<List<StockLevelDto>>> Handle(GetStockLevelsQuery request, CancellationToken cancellationToken)
        {
            var stocks = await _warehouseStockRepository.GetAllWithDetailsAsync();

            var stockLevels = stocks
                .Where(s => !request.WarehouseId.HasValue || s.WarehouseId == request.WarehouseId)
                .Where(s => !request.ProductId.HasValue || s.ProductId == request.ProductId)
                .Select(s => new StockLevelDto
                {
                    ProductId = s.ProductId,
                    ProductCode = s.Product?.Code ?? string.Empty,
                    ProductName = s.Product?.Name ?? string.Empty,
                    WarehouseId = s.WarehouseId,
                    WarehouseName = s.Warehouse?.Name ?? string.Empty,
                    AvailableQuantity = s.AvailableQuantity,
                    ReservedQuantity = s.ReservedQuantity,
                    TotalQuantity = s.AvailableQuantity + s.ReservedQuantity,
                    MinimumStock = s.Product?.MinimumStock,
                    ReorderLevel = s.Product?.ReorderLevel,
                    IsBelowMinimum = s.Product?.MinimumStock.HasValue == true &&
                                    s.AvailableQuantity < s.Product.MinimumStock.Value,
                    NeedsReorder = s.Product?.ReorderLevel.HasValue == true &&
                                  s.AvailableQuantity <= s.Product.ReorderLevel.Value
                })
                .Where(s => !(request.LowStockOnly ?? false) || s.IsBelowMinimum || s.NeedsReorder)
                .ToList();

            return Result<List<StockLevelDto>>.Success(stockLevels);
        }
    }
}
