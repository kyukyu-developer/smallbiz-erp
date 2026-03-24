

using ERP.Application.DTOs.Common;
using MediatR;
using ERP.Domain.Enums;
using ERP.Application.DTOs.Units;
using ERP.Application.DTOs.ProductGroup;

namespace ERP.Application.Features.ProductGroup.Commands
{
    public class CreateProductGroupCommand : IRequest<Result<ProductGroupDto>>
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;    

    }
}

