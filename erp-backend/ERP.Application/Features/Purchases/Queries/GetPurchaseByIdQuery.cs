using MediatR;
using ERP.Application.DTOs.Purchases;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Purchases.Queries
{
    public class GetPurchaseByIdQuery : IRequest<Result<PurchaseDto>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
