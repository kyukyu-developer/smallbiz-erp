using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Stock.Queries
{
    public class GetStockTransferByIdQueryHandler : IRequestHandler<GetStockTransferByIdQuery, Result<StockTransferDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStockTransferByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<StockTransferDto>> Handle(GetStockTransferByIdQuery request, CancellationToken cancellationToken)
        {
            var transfer = await _unitOfWork.StockTransfers.GetByIdAsync(request.Id);
            if (transfer == null)
                return Result<StockTransferDto>.Failure("Stock transfer not found.");

            var fromWarehouse = await _unitOfWork.Warehouses.GetByIdAsync(transfer.FromWarehouseId);
            var toWarehouse = await _unitOfWork.Warehouses.GetByIdAsync(transfer.ToWarehouseId);
            var product = await _unitOfWork.Products.GetByIdAsync(transfer.ProductId);

            var statusName = transfer.Status switch
            {
                0 => "Draft",
                1 => "Completed",
                2 => "Cancelled",
                _ => "Unknown"
            };

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
                StatusName = statusName,
                Notes = transfer.Notes,
                Active = transfer.Active,
                CreatedAt = transfer.CreatedAt,
                CreatedBy = transfer.CreatedBy
            });
        }
    }
}
