using MediatR;
using ERP.Application.DTOs.Common;
using ERP.Domain.Enums;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Purchases.Commands
{
    public class DeletePurchaseCommandHandler : IRequestHandler<DeletePurchaseCommand, Result<bool>>
    {
        private readonly IPurchaseRepository _purchaseRepository;

        public DeletePurchaseCommandHandler(IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        public async Task<Result<bool>> Handle(DeletePurchaseCommand request, CancellationToken cancellationToken)
        {
            var purchase = await _purchaseRepository.GetByIdAsync(request.Id);
            if (purchase == null)
                return Result<bool>.Failure("Purchase invoice not found.");

            if (purchase.Status != (int)PurchaseStatus.Draft)
                return Result<bool>.Failure("Only draft purchase invoices can be deleted.");

            purchase.Active = false;
            purchase.UpdatedAt = DateTime.UtcNow;
            purchase.LastAction = "DELETE";

            _purchaseRepository.Update(purchase);
            await _purchaseRepository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
