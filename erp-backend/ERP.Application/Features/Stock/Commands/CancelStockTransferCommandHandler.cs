using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Stock.Commands
{
    public class CancelStockTransferCommandHandler : IRequestHandler<CancelStockTransferCommand, Result<StockTransferDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CancelStockTransferCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<StockTransferDto>> Handle(CancelStockTransferCommand request, CancellationToken cancellationToken)
        {
            var transfer = await _unitOfWork.StockTransfers.GetByIdAsync(request.Id);
            if (transfer == null)
                return Result<StockTransferDto>.Failure("Stock transfer not found.");

            if (transfer.Status != 0) // Not Draft
                return Result<StockTransferDto>.Failure("Only draft transfers can be cancelled.");

            transfer.Status = 2; // Cancelled
            transfer.UpdatedAt = DateTime.UtcNow;
            transfer.LastAction = "CANCEL";
            _unitOfWork.StockTransfers.Update(transfer);
            await _unitOfWork.SaveChangesAsync();

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
                StatusName = "Cancelled",
                Notes = transfer.Notes,
                Active = transfer.Active,
                CreatedAt = transfer.CreatedAt,
                CreatedBy = transfer.CreatedBy
            });
        }
    }
}
