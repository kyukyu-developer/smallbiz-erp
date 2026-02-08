using ERP.Shared.Contracts.Common;
using Inventory.Application.DTOs.Products;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Features.Products.Commands;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
        if (product == null)
            return Result<ProductDto>.Failure("Product not found.");

        var dto = request.Product;

        // Validate category exists
        var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
        if (category == null)
            return Result<ProductDto>.Failure("Category not found.");

        // Validate unit exists
        var unit = await _unitOfWork.Units.GetByIdAsync(dto.BaseUnitId);
        if (unit == null)
            return Result<ProductDto>.Failure("Base unit not found.");

        // Check for duplicate code (excluding current product)
        var existing = await _unitOfWork.Products.FirstOrDefaultAsync(p => p.Code == dto.Code && p.Id != request.Id);
        if (existing != null)
            return Result<ProductDto>.Failure($"A product with code '{dto.Code}' already exists.");

        product.Code = dto.Code;
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.CategoryId = dto.CategoryId;
        product.BaseUnitId = dto.BaseUnitId;
        product.MinimumStock = dto.MinimumStock;
        product.MaximumStock = dto.MaximumStock;
        product.ReorderLevel = dto.ReorderLevel;
        product.IsActive = dto.IsActive;
        product.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Products.Update(product);
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
