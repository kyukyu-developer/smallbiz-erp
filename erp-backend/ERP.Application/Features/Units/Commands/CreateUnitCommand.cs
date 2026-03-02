using ERP.Application.DTOs.Common;


using MediatR;
using ERP.Domain.Enums;
using ERP.Application.DTOs.Units;

namespace ERP.Application.Features.Units.Commands
{
    public class CreateUnitCommand : IRequest<Result<UnitDto>>
    {
        public string Name { get; set; } = string.Empty;

        public string Symbol { get; set; } = string.Empty;     // e.g., pc, kg, bx

    }
}
