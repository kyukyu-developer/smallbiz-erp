using MediatR;
using ERP.Application.DTOs.Common;
using ERP.Domain.Enums;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.GoodsReceives.Commands
{
    public class CancelGoodsReceiveCommandHandler : IRequestHandler<CancelGoodsReceiveCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CancelGoodsReceiveCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(CancelGoodsReceiveCommand request, CancellationToken cancellationToken)
        {
            var grn = await _unitOfWork.GoodsReceives.GetByIdAsync(request.Id);
            if (grn == null)
                return Result<bool>.Failure("Goods receive not found.");

            if (grn.Status != (int)GoodsReceiveStatus.Draft)
                return Result<bool>.Failure("Only draft goods receives can be cancelled.");

            grn.Status = (int)GoodsReceiveStatus.Cancelled;
            grn.UpdatedAt = DateTime.UtcNow;
            grn.LastAction = "CANCEL";

            _unitOfWork.GoodsReceives.Update(grn);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
