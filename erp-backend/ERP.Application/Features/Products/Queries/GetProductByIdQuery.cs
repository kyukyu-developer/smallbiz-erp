using MediatR;
using ERP.Application.DTOs.Products;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Products.Queries
{
    public class GetProductByIdQuery : IRequest<Result<ProductDto>>
    {
        public int Id { get; set; }
    }
}
