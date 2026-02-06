using MediatR;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Products.Commands
{
    public class DeleteProductCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}
