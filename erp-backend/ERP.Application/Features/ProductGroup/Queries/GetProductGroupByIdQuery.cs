
using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.ProductGroup;
using MediatR;

namespace ERP.Application.Features.ProductGroup.Queries
{
    public class GetProductGroupByIdQuery : IRequest<Result<ProductGroupDto>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
