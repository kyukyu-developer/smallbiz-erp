using MediatR;
using ERP.Application.DTOs.Categories;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Categories.Queries
{
    public class GetCategoryByIdQuery : IRequest<Result<CategoryDto>>
    {
        public int Id { get; set; }
    }
}
