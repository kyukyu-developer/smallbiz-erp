using MediatR;
using ERP.Application.DTOs.PurchasePayments;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.PurchasePayments.Commands
{
    public class CreatePurchasePaymentCommandHandler : IRequestHandler<CreatePurchasePaymentCommand, Result<PurchasePaymentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePurchasePaymentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PurchasePaymentDto>> Handle(CreatePurchasePaymentCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _unitOfWork.Purchases.GetByIdAsync(request.PurchaseInvoiceId);
            if (invoice == null)
                return Result<PurchasePaymentDto>.Failure("Purchase invoice not found.");

            if (request.Amount <= 0)
                return Result<PurchasePaymentDto>.Failure("Payment amount must be greater than zero.");

            var paymentNumber = await GeneratePaymentNumberAsync();

            var payment = new PurchPayment
            {
                Id = Guid.NewGuid().ToString(),
                PaymentNumber = paymentNumber,
                PurchaseInvoiceId = request.PurchaseInvoiceId,
                Amount = request.Amount,
                PaymentDate = request.PaymentDate,
                PaymentMethod = (int)request.PaymentMethod,
                ReferenceNumber = request.ReferenceNumber,
                Notes = request.Notes,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                LastAction = "CREATE"
            };

            await _unitOfWork.PurchasePayments.AddAsync(payment);

            // Recalculate paid amount
            var allPayments = await _unitOfWork.PurchasePayments
                .FindAsync(p => p.PurchaseInvoiceId == request.PurchaseInvoiceId && p.Active);
            var totalPaid = allPayments.Sum(p => p.Amount) + request.Amount;

            invoice.PaidAmount = totalPaid;
            invoice.PaymentStatus = totalPaid >= invoice.TotalAmount
                ? (int)Domain.Enums.PaymentStatus.Paid
                : totalPaid > 0
                    ? (int)Domain.Enums.PaymentStatus.Partial
                    : (int)Domain.Enums.PaymentStatus.Pending;
            invoice.UpdatedAt = DateTime.UtcNow;
            invoice.LastAction = "PAYMENT";
            _unitOfWork.Purchases.Update(invoice);

            await _unitOfWork.SaveChangesAsync();

            return Result<PurchasePaymentDto>.Success(new PurchasePaymentDto
            {
                Id = payment.Id,
                PaymentNumber = payment.PaymentNumber,
                PurchaseInvoiceId = payment.PurchaseInvoiceId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                PaymentMethod = (Domain.Enums.PaymentMethod)payment.PaymentMethod,
                ReferenceNumber = payment.ReferenceNumber,
                Notes = payment.Notes,
                CreatedAt = payment.CreatedAt,
                CreatedBy = payment.CreatedBy
            });
        }

        private async Task<string> GeneratePaymentNumberAsync()
        {
            var year = DateTime.UtcNow.Year;
            var all = await _unitOfWork.PurchasePayments.GetAllAsync();
            var count = all.Count(p => p.CreatedAt.Year == year) + 1;
            return $"PP-{year}{count:D4}";
        }
    }
}
