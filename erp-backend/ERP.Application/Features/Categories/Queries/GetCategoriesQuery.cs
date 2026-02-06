using MediatR;
using ERP.Application.DTOs.Categories;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Categories.Queries
{
    public class GetCategoriesQuery : IRequest<Result<List<CategoryDto>>>
    {
        public int? ParentCategoryId { get; set; }
        public bool? IncludeInactive { get; set; }
    }
}
