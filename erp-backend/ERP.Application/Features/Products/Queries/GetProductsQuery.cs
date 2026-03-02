using MediatR;
using ERP.Application.DTOs.Products;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Products.Queries
{
    public class GetProductsQuery : IRequest<Result<List<ProductDto>>>
    {
        public string CategoryId { get; set; }= string.Empty;
        public string? SearchTerm { get; set; }
    }
}
