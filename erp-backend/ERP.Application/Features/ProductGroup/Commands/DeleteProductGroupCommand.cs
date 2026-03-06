
using MediatR;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.ProductGroup.Commands
{
    public class DeleteProductGroupCommand : IRequest<Result<bool>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
