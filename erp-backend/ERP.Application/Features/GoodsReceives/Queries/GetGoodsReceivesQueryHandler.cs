using MediatR;
using ERP.Application.DTOs.GoodsReceives;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.GoodsReceives.Queries
{
    public class GetGoodsReceivesQueryHandler : IRequestHandler<GetGoodsReceivesQuery, Result<List<GoodsReceiveDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetGoodsReceivesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<GoodsReceiveDto>>> Handle(GetGoodsReceivesQuery request, CancellationToken cancellationToken)
        {
            var all = await _unitOfWork.GoodsReceives.GetAllAsync();

            var filtered = all
                .Where(g => string.IsNullOrEmpty(request.SupplierId) || g.SupplierId == request.SupplierId)
                .Where(g => string.IsNullOrEmpty(request.WarehouseId) || g.WarehouseId == request.WarehouseId)
                .Where(g => !request.Status.HasValue || g.Status == (int)request.Status.Value)
                .Where(g => !request.StartDate.HasValue || g.ReceiveDate >= request.StartDate.Value)
                .Where(g => !request.EndDate.HasValue || g.ReceiveDate <= request.EndDate.Value)
                .OrderByDescending(g => g.ReceiveDate)
                .Select(g => Features.GoodsReceives.Commands.CreateGoodsReceiveCommandHandler.MapToDto(g))
                .ToList();

            return Result<List<GoodsReceiveDto>>.Success(filtered);
        }
    }
}
