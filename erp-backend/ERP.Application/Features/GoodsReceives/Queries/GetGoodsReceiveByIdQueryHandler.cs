using MediatR;
using ERP.Application.DTOs.GoodsReceives;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.GoodsReceives.Queries
{
    public class GetGoodsReceiveByIdQueryHandler : IRequestHandler<GetGoodsReceiveByIdQuery, Result<GoodsReceiveDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetGoodsReceiveByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GoodsReceiveDto>> Handle(GetGoodsReceiveByIdQuery request, CancellationToken cancellationToken)
        {
            var grn = await _unitOfWork.GoodsReceives.GetByIdAsync(request.Id);
            if (grn == null)
                return Result<GoodsReceiveDto>.Failure("Goods receive not found.");

            var items = await _unitOfWork.GoodsReceiveItems.FindAsync(i => i.GoodsReceiveId == grn.Id);
            grn.PurchGoodsReceiveItem = items.ToList();

            return Result<GoodsReceiveDto>.Success(
                Features.GoodsReceives.Commands.CreateGoodsReceiveCommandHandler.MapToDto(grn));
        }
    }
}
