
using MediatR;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Brands.Commands
{
    public class DeleteBrandCommand : IRequest<Result<bool>>
    {
        public string Id { get; set; }
    }
}
