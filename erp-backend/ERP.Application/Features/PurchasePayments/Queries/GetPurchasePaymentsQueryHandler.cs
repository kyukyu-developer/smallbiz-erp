using MediatR;
using ERP.Application.DTOs.PurchasePayments;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.PurchasePayments.Queries
{
    public class GetPurchasePaymentsQueryHandler : IRequestHandler<GetPurchasePaymentsQuery, Result<List<PurchasePaymentDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPurchasePaymentsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<PurchasePaymentDto>>> Handle(GetPurchasePaymentsQuery request, CancellationToken cancellationToken)
        {
            var payments = await _unitOfWork.PurchasePayments
                .FindAsync(p => p.PurchaseInvoiceId == request.PurchaseInvoiceId);

            var result = payments
                .OrderByDescending(p => p.PaymentDate)
                .Select(p => new PurchasePaymentDto
                {
                    Id = p.Id,
                    PaymentNumber = p.PaymentNumber,
                    PurchaseInvoiceId = p.PurchaseInvoiceId,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    PaymentMethod = (Domain.Enums.PaymentMethod)p.PaymentMethod,
                    ReferenceNumber = p.ReferenceNumber,
                    Notes = p.Notes,
                    CreatedAt = p.CreatedAt,
                    CreatedBy = p.CreatedBy
                })
                .ToList();

            return Result<List<PurchasePaymentDto>>.Success(result);
        }
    }
}
