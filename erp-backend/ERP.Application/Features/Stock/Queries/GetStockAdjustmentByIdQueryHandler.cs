using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Stock.Queries
{
    public class GetStockAdjustmentByIdQueryHandler : IRequestHandler<GetStockAdjustmentByIdQuery, Result<StockAdjustmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStockAdjustmentByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<StockAdjustmentDto>> Handle(GetStockAdjustmentByIdQuery request, CancellationToken cancellationToken)
        {
            var adjustment = await _unitOfWork.StockAdjustments.GetByIdAsync(request.Id);

            if (adjustment == null)
                return Result<StockAdjustmentDto>.Failure("Stock adjustment not found.");

            var product = await _unitOfWork.Products.GetByIdAsync(adjustment.ProductId);
            var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(adjustment.WarehouseId);

            return Result<StockAdjustmentDto>.Success(new StockAdjustmentDto
            {
                Id = adjustment.Id,
                AdjustmentNo = adjustment.AdjustmentNo,
                WarehouseId = adjustment.WarehouseId,
                WarehouseName = warehouse?.Name ?? string.Empty,
                ProductId = adjustment.ProductId,
                ProductCode = product?.Code ?? string.Empty,
                ProductName = product?.Name ?? string.Empty,
                AdjustmentQuantity = adjustment.AdjustmentQuantity,
                Reason = adjustment.Reason,
                AdjustmentDate = adjustment.AdjustmentDate,
                Active = adjustment.Active,
                CreatedAt = adjustment.CreatedAt,
                CreatedBy = adjustment.CreatedBy
            });
        }
    }
}
