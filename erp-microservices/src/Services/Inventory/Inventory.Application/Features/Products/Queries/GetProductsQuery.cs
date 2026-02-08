using ERP.Shared.Contracts.Common;
using Inventory.Application.DTOs.Products;
using MediatR;

namespace Inventory.Application.Features.Products.Queries;

public class GetProductsQuery : IRequest<Result<List<ProductDto>>>
{
}
