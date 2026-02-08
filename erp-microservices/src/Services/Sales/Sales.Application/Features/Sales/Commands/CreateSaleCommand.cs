using ERP.Shared.Contracts.Common;
using MediatR;
using Sales.Application.DTOs.Sales;

namespace Sales.Application.Features.Sales.Commands;

public class CreateSaleCommand : IRequest<Result<SaleDto>>
{
    public CreateSaleDto Dto { get; set; } = null!;
}
