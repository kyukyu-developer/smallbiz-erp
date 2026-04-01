using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Stock.Commands
{
    public class CreateStockAdjustmentCommandHandler : IRequestHandler<CreateStockAdjustmentCommand, Result<StockAdjustmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateStockAdjustmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<StockAdjustmentDto>> Handle(CreateStockAdjustmentCommand request, CancellationToken cancellationToken)
        {
            // Validate warehouse exists
            var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(request.WarehouseId);
            if (warehouse == null)
                return Result<StockAdjustmentDto>.Failure("Warehouse not found.");

            // Validate product exists
            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null)
                return Result<StockAdjustmentDto>.Failure("Product not found.");

            // Check stock for negative adjustment
            if (request.AdjustmentQuantity < 0)
            {
                var currentStock = await _unitOfWork.WarehouseStocks
                    .FirstOrDefaultAsync(ws => ws.WarehouseId == request.WarehouseId && ws.ProductId == request.ProductId);

                var available = currentStock?.AvailableQuantity ?? 0;
                if (available + request.AdjustmentQuantity < 0 && !(product.AllowNegativeStock ?? false))
                    return Result<StockAdjustmentDto>.Failure($"Insufficient stock. Available: {available}, Adjustment: {request.AdjustmentQuantity}");
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Generate adjustment number
                var adjustmentNo = await GenerateAdjustmentNumberAsync();

                // Create adjustment record
                var adjustment = new InvStockAdjustment
                {
                    Id = Guid.NewGuid().ToString(),
                    AdjustmentNo = adjustmentNo,
                    WarehouseId = request.WarehouseId,
                    ProductId = request.ProductId,
                    AdjustmentQuantity = request.AdjustmentQuantity,
                    Reason = request.Reason,
                    AdjustmentDate = request.AdjustmentDate,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    LastAction = "CREATE"
                };

                await _unitOfWork.StockAdjustments.AddAsync(adjustment);

                // Create stock movement record
                var movement = new InvStockMovement
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = request.ProductId,
                    WarehouseId = request.WarehouseId,
                    MovementType = request.AdjustmentQuantity >= 0
                        ? ((int)MovementType.In).ToString()
                        : ((int)MovementType.Out).ToString(),
                    ReferenceType = (int)ReferenceType.Adjustment,
                    ReferenceId = adjustment.Id,
                    BaseQuantity = Math.Abs(request.AdjustmentQuantity),
                    MovementDate = request.AdjustmentDate,
                    Notes = $"Stock Adjustment: {adjustmentNo} - {request.Reason}",
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    LastAction = "CREATE"
                };

                await _unitOfWork.StockMovements.AddAsync(movement);

                // Update warehouse stock
                var warehouseStock = await _unitOfWork.WarehouseStocks
                    .FirstOrDefaultAsync(ws => ws.WarehouseId == request.WarehouseId && ws.ProductId == request.ProductId);

                if (warehouseStock != null)
                {
                    warehouseStock.AvailableQuantity += request.AdjustmentQuantity;
                    warehouseStock.UpdatedAt = DateTime.UtcNow;
                    warehouseStock.LastAction = "ADJUSTMENT";
                    _unitOfWork.WarehouseStocks.Update(warehouseStock);
                }
                else
                {
                    // Create new stock record if doesn't exist
                    var newStock = new InvWarehouseStock
                    {
                        Id = Guid.NewGuid().ToString(),
                        WarehouseId = request.WarehouseId,
                        ProductId = request.ProductId,
                        AvailableQuantity = request.AdjustmentQuantity,
                        ReservedQuantity = 0,
                        Active = true,
                        CreatedAt = DateTime.UtcNow,
                        LastAction = "ADJUSTMENT"
                    };
                    await _unitOfWork.WarehouseStocks.AddAsync(newStock);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return Result<StockAdjustmentDto>.Success(new StockAdjustmentDto
                {
                    Id = adjustment.Id,
                    AdjustmentNo = adjustment.AdjustmentNo,
                    WarehouseId = adjustment.WarehouseId,
                    WarehouseName = warehouse.Name,
                    ProductId = adjustment.ProductId,
                    ProductCode = product.Code,
                    ProductName = product.Name,
                    AdjustmentQuantity = adjustment.AdjustmentQuantity,
                    Reason = adjustment.Reason,
                    AdjustmentDate = adjustment.AdjustmentDate,
                    Active = adjustment.Active,
                    CreatedAt = adjustment.CreatedAt,
                    CreatedBy = adjustment.CreatedBy
                });
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result<StockAdjustmentDto>.Failure($"Failed to create stock adjustment: {ex.Message}");
            }
        }

        private async Task<string> GenerateAdjustmentNumberAsync()
        {
            var year = DateTime.UtcNow.Year;
            var allAdjustments = await _unitOfWork.StockAdjustments.GetAllAsync();
            var count = allAdjustments.Count(a => a.CreatedAt.Year == year) + 1;
            return $"ADJ-{year}{count:D4}";
        }
    }
}
