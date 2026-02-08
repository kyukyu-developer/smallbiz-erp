using ERP.Shared.Contracts.Common;
using Inventory.Application.DTOs.Products;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Features.Products.Commands;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Product;

        // Validate category exists
        var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
        if (category == null)
            return Result<ProductDto>.Failure("Category not found.");

        // Validate unit exists
        var unit = await _unitOfWork.Units.GetByIdAsync(dto.BaseUnitId);
        if (unit == null)
            return Result<ProductDto>.Failure("Base unit not found.");

        // Check for duplicate code
        var existing = await _unitOfWork.Products.FirstOrDefaultAsync(p => p.Code == dto.Code);
        if (existing != null)
            return Result<ProductDto>.Failure($"A product with code '{dto.Code}' already exists.");

        var product = new Product
        {
            Code = dto.Code,
            Name = dto.Name,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            BaseUnitId = dto.BaseUnitId,
            MinimumStock = dto.MinimumStock,
            MaximumStock = dto.MaximumStock,
            ReorderLevel = dto.ReorderLevel,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        var result = new ProductDto
        {
            Id = product.Id,
            Code = product.Code,
            Name = product.Name,
            Description = product.Description,
            CategoryId = product.CategoryId,
            CategoryName = category.Name,
            BaseUnitId = product.BaseUnitId,
            BaseUnitName = unit.Name,
            MinimumStock = product.MinimumStock,
            MaximumStock = product.MaximumStock,
            ReorderLevel = product.ReorderLevel,
            IsActive = product.IsActive
        };

        return Result<ProductDto>.Success(result);
    }
}
