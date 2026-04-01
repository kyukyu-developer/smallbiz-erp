using MediatR;
using ERP.Application.DTOs.Stock;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Stock.Queries
{
    public class GetStockMovementsQuery : IRequest<Result<List<StockMovementDto>>>
    {
        public string? WarehouseId { get; set; }
        public string? ProductId { get; set; }
        public string? MovementType { get; set; }
        public int? ReferenceType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
