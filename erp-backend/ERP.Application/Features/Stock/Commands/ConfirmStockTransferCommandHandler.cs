using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Stock.Commands
{
    public class ConfirmStockTransferCommandHandler : IRequestHandler<ConfirmStockTransferCommand, Result<StockTransferDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmStockTransferCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<StockTransferDto>> Handle(ConfirmStockTransferCommand request, CancellationToken cancellationToken)
        {
            var transfer = await _unitOfWork.StockTransfers.GetByIdAsync(request.Id);
            if (transfer == null)
                return Result<StockTransferDto>.Failure("Stock transfer not found.");

            if (transfer.Status != 0) // Not Draft
                return Result<StockTransferDto>.Failure("Only draft transfers can be confirmed.");

            // Check source warehouse stock
            var sourceStock = await _unitOfWork.WarehouseStocks
                .FirstOrDefaultAsync(ws => ws.WarehouseId == transfer.FromWarehouseId && ws.ProductId == transfer.ProductId);

            var available = sourceStock?.AvailableQuantity ?? 0;
            if (available < transfer.Quantity)
                return Result<StockTransferDto>.Failure($"Insufficient stock in source warehouse. Available: {available}, Required: {transfer.Quantity}");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Update transfer status
                transfer.Status = 1; // Completed
                transfer.UpdatedAt = DateTime.UtcNow;
                transfer.LastAction = "CONFIRM";
                _unitOfWork.StockTransfers.Update(transfer);

                // Create TRANSFER_OUT movement (source)
                var movementOut = new InvStockMovement
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = transfer.ProductId,
                    WarehouseId = transfer.FromWarehouseId,
                    MovementType = "TRANSFER_OUT",
                    ReferenceType = (int)ReferenceType.Transfer,
                    ReferenceId = transfer.Id,
                    BaseQuantity = transfer.Quantity,
                    MovementDate = transfer.TransferDate,
                    Notes = $"Stock Transfer Out: {transfer.TransferNo}",
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    LastAction = "CREATE"
                };
                await _unitOfWork.StockMovements.AddAsync(movementOut);

                // Create TRANSFER_IN movement (destination)
                var movementIn = new InvStockMovement
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = transfer.ProductId,
                    WarehouseId = transfer.ToWarehouseId,
                    MovementType = "TRANSFER_IN",
                    ReferenceType = (int)ReferenceType.Transfer,
                    ReferenceId = transfer.Id,
                    BaseQuantity = transfer.Quantity,
                    MovementDate = transfer.TransferDate,
                    Notes = $"Stock Transfer In: {transfer.TransferNo}",
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    LastAction = "CREATE"
                };
                await _unitOfWork.StockMovements.AddAsync(movementIn);

                // Deduct from source warehouse
                sourceStock!.AvailableQuantity -= transfer.Quantity;
                sourceStock.UpdatedAt = DateTime.UtcNow;
                sourceStock.LastAction = "TRANSFER_OUT";
                _unitOfWork.WarehouseStocks.Update(sourceStock);

                // Add to destination warehouse
                var destStock = await _unitOfWork.WarehouseStocks
                    .FirstOrDefaultAsync(ws => ws.WarehouseId == transfer.ToWarehouseId && ws.ProductId == transfer.ProductId);

                if (destStock != null)
                {
                    destStock.AvailableQuantity += transfer.Quantity;
                    destStock.UpdatedAt = DateTime.UtcNow;
                    destStock.LastAction = "TRANSFER_IN";
                    _unitOfWork.WarehouseStocks.Update(destStock);
                }
                else
                {
                    var newStock = new InvWarehouseStock
                    {
                        Id = Guid.NewGuid().ToString(),
                        WarehouseId = transfer.ToWarehouseId,
                        ProductId = transfer.ProductId,
                        AvailableQuantity = transfer.Quantity,
                        ReservedQuantity = 0,
                        Active = true,
                        CreatedAt = DateTime.UtcNow,
                        LastAction = "TRANSFER_IN"
                    };
                    await _unitOfWork.WarehouseStocks.AddAsync(newStock);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                var fromWarehouse = await _unitOfWork.Warehouses.GetByIdAsync(transfer.FromWarehouseId);
                var toWarehouse = await _unitOfWork.Warehouses.GetByIdAsync(transfer.ToWarehouseId);
                var product = await _unitOfWork.Products.GetByIdAsync(transfer.ProductId);

                return Result<StockTransferDto>.Success(new StockTransferDto
                {
                    Id = transfer.Id,
                    TransferNo = transfer.TransferNo,
                    FromWarehouseId = transfer.FromWarehouseId,
                    FromWarehouseName = fromWarehouse?.Name ?? string.Empty,
                    ToWarehouseId = transfer.ToWarehouseId,
                    ToWarehouseName = toWarehouse?.Name ?? string.Empty,
                    ProductId = transfer.ProductId,
                    ProductCode = product?.Code ?? string.Empty,
                    ProductName = product?.Name ?? string.Empty,
                    Quantity = transfer.Quantity,
                    TransferDate = transfer.TransferDate,
                    Status = transfer.Status,
                    StatusName = "Completed",
                    Notes = transfer.Notes,
                    Active = transfer.Active,
                    CreatedAt = transfer.CreatedAt,
                    CreatedBy = transfer.CreatedBy
                });
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result<StockTransferDto>.Failure($"Failed to confirm stock transfer: {ex.Message}");
            }
        }
    }
}
