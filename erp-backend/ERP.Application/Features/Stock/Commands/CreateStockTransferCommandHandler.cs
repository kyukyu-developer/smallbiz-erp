using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Stock.Commands
{
    public class CreateStockTransferCommandHandler : IRequestHandler<CreateStockTransferCommand, Result<StockTransferDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateStockTransferCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<StockTransferDto>> Handle(CreateStockTransferCommand request, CancellationToken cancellationToken)
        {
            // Validate
            if (request.FromWarehouseId == request.ToWarehouseId)
                return Result<StockTransferDto>.Failure("Source and destination warehouse cannot be the same.");

            var fromWarehouse = await _unitOfWork.Warehouses.GetByIdAsync(request.FromWarehouseId);
            if (fromWarehouse == null)
                return Result<StockTransferDto>.Failure("Source warehouse not found.");

            var toWarehouse = await _unitOfWork.Warehouses.GetByIdAsync(request.ToWarehouseId);
            if (toWarehouse == null)
                return Result<StockTransferDto>.Failure("Destination warehouse not found.");

            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
            if (product == null)
                return Result<StockTransferDto>.Failure("Product not found.");

            if (request.Quantity <= 0)
                return Result<StockTransferDto>.Failure("Transfer quantity must be greater than zero.");

            // Generate transfer number
            var transferNo = await GenerateTransferNumberAsync();

            // Create transfer as Draft (status = 0)
            var transfer = new InvStockTransfer
            {
                Id = Guid.NewGuid().ToString(),
                TransferNo = transferNo,
                FromWarehouseId = request.FromWarehouseId,
                ToWarehouseId = request.ToWarehouseId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                TransferDate = request.TransferDate,
                Status = 0, // Draft
                Notes = request.Notes,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                LastAction = "CREATE"
            };

            await _unitOfWork.StockTransfers.AddAsync(transfer);
            await _unitOfWork.SaveChangesAsync();

            return Result<StockTransferDto>.Success(new StockTransferDto
            {
                Id = transfer.Id,
                TransferNo = transfer.TransferNo,
                FromWarehouseId = transfer.FromWarehouseId,
                FromWarehouseName = fromWarehouse.Name,
                ToWarehouseId = transfer.ToWarehouseId,
                ToWarehouseName = toWarehouse.Name,
                ProductId = transfer.ProductId,
                ProductCode = product.Code,
                ProductName = product.Name,
                Quantity = transfer.Quantity,
                TransferDate = transfer.TransferDate,
                Status = transfer.Status,
                StatusName = "Draft",
                Notes = transfer.Notes,
                Active = transfer.Active,
                CreatedAt = transfer.CreatedAt,
                CreatedBy = transfer.CreatedBy
            });
        }

        private async Task<string> GenerateTransferNumberAsync()
        {
            var year = DateTime.UtcNow.Year;
            var allTransfers = await _unitOfWork.StockTransfers.GetAllAsync();
            var count = allTransfers.Count(t => t.CreatedAt.Year == year) + 1;
            return $"TRF-{year}{count:D4}";
        }
    }
}
