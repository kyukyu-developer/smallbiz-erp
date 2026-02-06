using MediatR;
using ERP.Application.DTOs.Purchases;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Purchases.Queries
{
    public class GetPurchaseByIdQuery : IRequest<Result<PurchaseDto>>
    {
        public int Id { get; set; }
    }
}
