
using MediatR;
using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.Brands;

namespace ERP.Application.Features.Brands.Queries
{
    public class GetBrandByIdQuery : IRequest<Result<BrandDto>>
    {
        public string Id { get; set; }
    }
}
