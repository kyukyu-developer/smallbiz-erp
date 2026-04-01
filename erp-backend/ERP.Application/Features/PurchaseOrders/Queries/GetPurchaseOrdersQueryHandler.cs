using MediatR;
using ERP.Application.DTOs.PurchaseOrders;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.PurchaseOrders.Queries
{
    public class GetPurchaseOrdersQueryHandler : IRequestHandler<GetPurchaseOrdersQuery, Result<List<PurchaseOrderDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPurchaseOrdersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<PurchaseOrderDto>>> Handle(GetPurchaseOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.PurchaseOrders.GetAllAsync();

            var filtered = orders
                .Where(o => string.IsNullOrEmpty(request.SupplierId) || o.SupplierId == request.SupplierId)
                .Where(o => !request.Status.HasValue || o.Status == (int)request.Status.Value)
                .Where(o => !request.StartDate.HasValue || o.OrderDate >= request.StartDate.Value)
                .Where(o => !request.EndDate.HasValue || o.OrderDate <= request.EndDate.Value)
                .OrderByDescending(o => o.OrderDate)
                .ThenByDescending(o => o.CreatedAt)
                .Select(o => Features.PurchaseOrders.Commands.CreatePurchaseOrderCommandHandler.MapToDto(o))
                .ToList();

            return Result<List<PurchaseOrderDto>>.Success(filtered);
        }
    }
}
