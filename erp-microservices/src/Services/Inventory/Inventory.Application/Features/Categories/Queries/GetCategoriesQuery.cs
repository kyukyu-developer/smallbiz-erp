using ERP.Shared.Contracts.Common;
using Inventory.Application.DTOs.Categories;
using MediatR;

namespace Inventory.Application.Features.Categories.Queries;

public class GetCategoriesQuery : IRequest<Result<List<CategoryDto>>>
{
}
