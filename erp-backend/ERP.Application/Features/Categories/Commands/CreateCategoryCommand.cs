using MediatR;
using ERP.Application.DTOs.Categories;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Categories.Commands
{
    public class CreateCategoryCommand : IRequest<Result<CategoryDto>>
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
