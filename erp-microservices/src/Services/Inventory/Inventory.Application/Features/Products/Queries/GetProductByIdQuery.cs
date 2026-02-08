using ERP.Shared.Contracts.Common;
using Inventory.Application.DTOs.Products;
using MediatR;

namespace Inventory.Application.Features.Products.Queries;

public class GetProductByIdQuery : IRequest<Result<ProductDto>>
{
    public int Id { get; set; }
}
