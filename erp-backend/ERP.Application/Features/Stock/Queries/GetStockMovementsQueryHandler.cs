using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Stock.Queries
{
    public class GetStockMovementsQueryHandler : IRequestHandler<GetStockMovementsQuery, Result<List<StockMovementDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStockMovementsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<StockMovementDto>>> Handle(GetStockMovementsQuery request, CancellationToken cancellationToken)
        {
            var movements = await _unitOfWork.StockMovements.GetAllAsync();

            var filtered = movements
                .Where(m => string.IsNullOrEmpty(request.WarehouseId) || m.WarehouseId == request.WarehouseId)
                .Where(m => string.IsNullOrEmpty(request.ProductId) || m.ProductId == request.ProductId)
                .Where(m => string.IsNullOrEmpty(request.MovementType) || m.MovementType == request.MovementType)
                .Where(m => !request.ReferenceType.HasValue || m.ReferenceType == request.ReferenceType.Value)
                .Where(m => !request.StartDate.HasValue || m.MovementDate >= request.StartDate.Value)
                .Where(m => !request.EndDate.HasValue || m.MovementDate <= request.EndDate.Value)
                .OrderByDescending(m => m.MovementDate)
                .ThenByDescending(m => m.CreatedAt)
                .Select(m => new StockMovementDto
                {
                    Id = m.Id,
                    ProductId = m.ProductId,
                    ProductCode = m.Product?.Code ?? string.Empty,
                    ProductName = m.Product?.Name ?? string.Empty,
                    WarehouseId = m.WarehouseId,
                    WarehouseName = m.Warehouse?.Name ?? string.Empty,
                    MovementType = m.MovementType,
                    ReferenceType = m.ReferenceType,
                    ReferenceTypeName = GetReferenceTypeName(m.ReferenceType),
                    ReferenceId = m.ReferenceId,
                    BaseQuantity = m.BaseQuantity,
                    BatchId = m.BatchId,
                    BatchNo = m.Batch?.BatchNo,
                    SerialId = m.SerialId,
                    SerialNo = m.Serial?.SerialNo,
                    MovementDate = m.MovementDate,
                    Notes = m.Notes,
                    CreatedAt = m.CreatedAt,
                    CreatedBy = m.CreatedBy
                })
                .ToList();

            return Result<List<StockMovementDto>>.Success(filtered);
        }

        private static string GetReferenceTypeName(int referenceType)
        {
            return referenceType switch
            {
                1 => "Purchase",
                2 => "Sale",
                3 => "Adjustment",
                4 => "Transfer",
                5 => "Return",
                _ => "Unknown"
            };
        }
    }
}
