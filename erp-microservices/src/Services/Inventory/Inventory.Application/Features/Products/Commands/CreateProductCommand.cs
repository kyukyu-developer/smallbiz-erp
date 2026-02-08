using ERP.Shared.Contracts.Common;
using Inventory.Application.DTOs.Products;
using MediatR;

namespace Inventory.Application.Features.Products.Commands;

public class CreateProductCommand : IRequest<Result<ProductDto>>
{
    public CreateProductDto Product { get; set; } = null!;
}
