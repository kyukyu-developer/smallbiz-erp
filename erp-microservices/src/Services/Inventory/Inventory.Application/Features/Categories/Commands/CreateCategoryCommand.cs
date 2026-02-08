using ERP.Shared.Contracts.Common;
using Inventory.Application.DTOs.Categories;
using MediatR;

namespace Inventory.Application.Features.Categories.Commands;

public class CreateCategoryCommand : IRequest<Result<CategoryDto>>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ParentCategoryId { get; set; }
}
