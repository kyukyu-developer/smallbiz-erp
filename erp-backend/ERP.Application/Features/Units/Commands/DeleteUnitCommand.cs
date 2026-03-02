

using MediatR;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Units.Commands
{
    public class DeleteUnitCommand : IRequest<Result<bool>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
