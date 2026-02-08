using ERP.Shared.Contracts.Common;
using Inventory.Application.DTOs.Products;
using MediatR;

namespace Inventory.Application.Features.Products.Commands;

public class UpdateProductCommand : IRequest<Result<ProductDto>>
{
    public int Id { get; set; }
    public UpdateProductDto Product { get; set; } = null!;
}
