using MediatR;
using ERP.Application.DTOs.Categories;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Categories.Queries
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Result<List<CategoryDto>>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<List<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllAsync();

            var filteredCategories = categories
                .Where(c => (request.IncludeInactive ?? false) || c.IsActive)
                .Where(c => !request.ParentCategoryId.HasValue || c.ParentCategoryId == request.ParentCategoryId)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    Description = c.Description,
                    ParentCategoryId = c.ParentCategoryId,
                    IsActive = c.IsActive
                })
                .ToList();

            return Result<List<CategoryDto>>.Success(filteredCategories);
        }
    }
}
