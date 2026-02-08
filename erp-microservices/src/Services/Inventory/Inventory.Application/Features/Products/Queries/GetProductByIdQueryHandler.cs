using ERP.Shared.Contracts.Common;
using Inventory.Application.DTOs.Products;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Features.Products.Queries;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(
            request.Id,
            p => p.Category,
            p => p.BaseUnit);

        if (product == null)
            return Result<ProductDto>.Failure("Product not found.");

        var result = new ProductDto
        {
            Id = product.Id,
            Code = product.Code,
            Name = product.Name,
            Description = product.Description,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty,
            BaseUnitId = product.BaseUnitId,
            BaseUnitName = product.BaseUnit?.Name ?? string.Empty,
            MinimumStock = product.MinimumStock,
            MaximumStock = product.MaximumStock,
            ReorderLevel = product.ReorderLevel,
            IsActive = product.IsActive
        };

        return Result<ProductDto>.Success(result);
    }
}
