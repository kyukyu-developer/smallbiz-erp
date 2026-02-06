using MediatR;
using ERP.Application.DTOs.Products;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Products.Queries
{
    public class GetProductsQuery : IRequest<Result<List<ProductDto>>>
    {
        public int? CategoryId { get; set; }
        public string? SearchTerm { get; set; }
    }
}
