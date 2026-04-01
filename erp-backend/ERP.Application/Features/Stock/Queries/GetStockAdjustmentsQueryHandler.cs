using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Stock.Queries
{
    public class GetStockAdjustmentsQueryHandler : IRequestHandler<GetStockAdjustmentsQuery, Result<List<StockAdjustmentDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStockAdjustmentsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<StockAdjustmentDto>>> Handle(GetStockAdjustmentsQuery request, CancellationToken cancellationToken)
        {
            var adjustments = await _unitOfWork.StockAdjustments.GetAllAsync();

            var filtered = adjustments
                .Where(a => string.IsNullOrEmpty(request.WarehouseId) || a.WarehouseId == request.WarehouseId)
                .Where(a => string.IsNullOrEmpty(request.ProductId) || a.ProductId == request.ProductId)
                .Where(a => !request.StartDate.HasValue || a.AdjustmentDate >= request.StartDate.Value)
                .Where(a => !request.EndDate.HasValue || a.AdjustmentDate <= request.EndDate.Value)
                .OrderByDescending(a => a.AdjustmentDate)
                .ThenByDescending(a => a.CreatedAt)
                .Select(a => new StockAdjustmentDto
                {
                    Id = a.Id,
                    AdjustmentNo = a.AdjustmentNo,
                    WarehouseId = a.WarehouseId,
                    WarehouseName = a.Warehouse?.Name ?? string.Empty,
                    ProductId = a.ProductId,
                    ProductCode = a.Product?.Code ?? string.Empty,
                    ProductName = a.Product?.Name ?? string.Empty,
                    AdjustmentQuantity = a.AdjustmentQuantity,
                    Reason = a.Reason,
                    AdjustmentDate = a.AdjustmentDate,
                    Active = a.Active,
                    CreatedAt = a.CreatedAt,
                    CreatedBy = a.CreatedBy
                })
                .ToList();

            return Result<List<StockAdjustmentDto>>.Success(filtered);
        }
    }
}
