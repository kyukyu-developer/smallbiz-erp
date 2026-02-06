using MediatR;
using ERP.Application.DTOs.Sales;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Sales.Queries
{
    public class GetSaleByIdQuery : IRequest<Result<SaleDto>>
    {
        public int Id { get; set; }
    }
}
