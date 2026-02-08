using ERP.Shared.Contracts.Common;
using Inventory.Application.DTOs.Products;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Features.Products.Queries;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<List<ProductDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _unitOfWork.Products.GetAllAsync();
        var categories = (await _unitOfWork.Categories.GetAllAsync()).ToDictionary(c => c.Id, c => c.Name);
        var units = (await _unitOfWork.Units.GetAllAsync()).ToDictionary(u => u.Id, u => u.Name);

        var result = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Code = p.Code,
            Name = p.Name,
            Description = p.Description,
            CategoryId = p.CategoryId,
            CategoryName = categories.GetValueOrDefault(p.CategoryId, string.Empty),
            BaseUnitId = p.BaseUnitId,
            BaseUnitName = units.GetValueOrDefault(p.BaseUnitId, string.Empty),
            MinimumStock = p.MinimumStock,
            MaximumStock = p.MaximumStock,
            ReorderLevel = p.ReorderLevel,
            IsActive = p.IsActive
        }).ToList();

        return Result<List<ProductDto>>.Success(result);
    }
}
