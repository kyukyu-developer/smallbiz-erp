using MediatR;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Products.Commands
{
    public class DeleteProductCommand : IRequest<Result<bool>>
    {
        public string Id { get; set; }
    }
}
