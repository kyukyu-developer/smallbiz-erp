

using MediatR;
using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.Brands;

namespace ERP.Application.Features.Brands.Commands
{
    public class CreateBrandCommand : IRequest<Result<BrandDto>>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool Active { get; set; } = true;
    }
}
