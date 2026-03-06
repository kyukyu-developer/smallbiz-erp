
using MediatR;
using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.ProductGroup;

namespace ERP.Application.Features.ProductGroup.Commands
{
    public class UpdateProductGroupCommand : IRequest<Result<ProductGroupDto>>
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public bool Active { get; set; } = true;

    }
}
