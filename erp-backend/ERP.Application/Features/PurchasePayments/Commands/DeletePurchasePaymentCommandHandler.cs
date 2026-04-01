using MediatR;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.PurchasePayments.Commands
{
    public class DeletePurchasePaymentCommandHandler : IRequestHandler<DeletePurchasePaymentCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePurchasePaymentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeletePurchasePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _unitOfWork.PurchasePayments.GetByIdAsync(request.PaymentId);
            if (payment == null)
                return Result<bool>.Failure("Payment not found.");

            if (payment.PurchaseInvoiceId != request.PurchaseInvoiceId)
                return Result<bool>.Failure("Payment does not belong to this invoice.");

            payment.Active = false;
            payment.UpdatedAt = DateTime.UtcNow;
            payment.LastAction = "DELETE";
            _unitOfWork.PurchasePayments.Update(payment);

            // Recalculate
            var invoice = await _unitOfWork.Purchases.GetByIdAsync(request.PurchaseInvoiceId);
            if (invoice != null)
            {
                var activePayments = await _unitOfWork.PurchasePayments
                    .FindAsync(p => p.PurchaseInvoiceId == request.PurchaseInvoiceId && p.Active && p.Id != request.PaymentId);
                var totalPaid = activePayments.Sum(p => p.Amount);

                invoice.PaidAmount = totalPaid;
                invoice.PaymentStatus = totalPaid >= invoice.TotalAmount
                    ? (int)Domain.Enums.PaymentStatus.Paid
                    : totalPaid > 0
                        ? (int)Domain.Enums.PaymentStatus.Partial
                        : (int)Domain.Enums.PaymentStatus.Pending;
                invoice.UpdatedAt = DateTime.UtcNow;
                invoice.LastAction = "PAYMENT_DELETE";
                _unitOfWork.Purchases.Update(invoice);
            }

            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
