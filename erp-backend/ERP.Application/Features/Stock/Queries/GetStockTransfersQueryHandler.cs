using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Stock.Queries
{
    public class GetStockTransfersQueryHandler : IRequestHandler<GetStockTransfersQuery, Result<List<StockTransferDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStockTransfersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<StockTransferDto>>> Handle(GetStockTransfersQuery request, CancellationToken cancellationToken)
        {
            var transfers = await _unitOfWork.StockTransfers.GetAllAsync();

            var filtered = transfers
                .Where(t => string.IsNullOrEmpty(request.FromWarehouseId) || t.FromWarehouseId == request.FromWarehouseId)
                .Where(t => string.IsNullOrEmpty(request.ToWarehouseId) || t.ToWarehouseId == request.ToWarehouseId)
                .Where(t => string.IsNullOrEmpty(request.ProductId) || t.ProductId == request.ProductId)
                .Where(t => !request.Status.HasValue || t.Status == request.Status.Value)
                .Where(t => !request.StartDate.HasValue || t.TransferDate >= request.StartDate.Value)
                .Where(t => !request.EndDate.HasValue || t.TransferDate <= request.EndDate.Value)
                .OrderByDescending(t => t.TransferDate)
                .ThenByDescending(t => t.CreatedAt)
                .Select(t => new StockTransferDto
                {
                    Id = t.Id,
                    TransferNo = t.TransferNo,
                    FromWarehouseId = t.FromWarehouseId,
                    FromWarehouseName = t.FromWarehouse?.Name ?? string.Empty,
                    ToWarehouseId = t.ToWarehouseId,
                    ToWarehouseName = t.ToWarehouse?.Name ?? string.Empty,
                    ProductId = t.ProductId,
                    ProductCode = t.Product?.Code ?? string.Empty,
                    ProductName = t.Product?.Name ?? string.Empty,
                    Quantity = t.Quantity,
                    TransferDate = t.TransferDate,
                    Status = t.Status,
                    StatusName = GetStatusName(t.Status),
                    Notes = t.Notes,
                    Active = t.Active,
                    CreatedAt = t.CreatedAt,
                    CreatedBy = t.CreatedBy
                })
                .ToList();

            return Result<List<StockTransferDto>>.Success(filtered);
        }

        private static string GetStatusName(int status) => status switch
        {
            0 => "Draft",
            1 => "Completed",
            2 => "Cancelled",
            _ => "Unknown"
        };
    }
}
