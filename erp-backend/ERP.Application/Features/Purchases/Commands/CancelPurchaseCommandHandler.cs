using MediatR;
using ERP.Application.DTOs.Common;
using ERP.Domain.Enums;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Purchases.Commands
{
    public class CancelPurchaseCommandHandler : IRequestHandler<CancelPurchaseCommand, Result<bool>>
    {
        private readonly IPurchaseRepository _purchaseRepository;

        public CancelPurchaseCommandHandler(IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        public async Task<Result<bool>> Handle(CancelPurchaseCommand request, CancellationToken cancellationToken)
        {
            var purchase = await _purchaseRepository.GetByIdAsync(request.Id);
            if (purchase == null)
                return Result<bool>.Failure("Purchase invoice not found.");

            if (purchase.Status == (int)PurchaseStatus.Cancelled)
                return Result<bool>.Failure("Purchase invoice is already cancelled.");

            purchase.Status = (int)PurchaseStatus.Cancelled;
            purchase.UpdatedAt = DateTime.UtcNow;
            purchase.LastAction = "CANCEL";

            _purchaseRepository.Update(purchase);
            await _purchaseRepository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
