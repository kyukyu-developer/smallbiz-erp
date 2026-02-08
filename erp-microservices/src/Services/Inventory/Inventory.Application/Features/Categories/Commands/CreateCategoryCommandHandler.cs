using ERP.Shared.Contracts.Common;
using Inventory.Application.DTOs.Categories;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Features.Categories.Commands;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        // Validate parent category if provided
        if (request.ParentCategoryId.HasValue)
        {
            var parentCategory = await _unitOfWork.Categories.GetByIdAsync(request.ParentCategoryId.Value);
            if (parentCategory == null)
                return Result<CategoryDto>.Failure("Parent category not found.");
        }

        // Check for duplicate name
        var existing = await _unitOfWork.Categories.FirstOrDefaultAsync(c => c.Name == request.Name);
        if (existing != null)
            return Result<CategoryDto>.Failure($"A category with name '{request.Name}' already exists.");

        var category = new Category
        {
            Name = request.Name,
            Description = request.Description,
            ParentCategoryId = request.ParentCategoryId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        var result = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ParentCategoryId = category.ParentCategoryId,
            IsActive = category.IsActive
        };

        return Result<CategoryDto>.Success(result);
    }
}
