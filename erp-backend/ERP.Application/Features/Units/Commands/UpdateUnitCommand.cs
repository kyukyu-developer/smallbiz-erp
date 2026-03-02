

using MediatR;
using ERP.Application.DTOs.Warehouses;
using ERP.Application.DTOs.Common;
using ERP.Domain.Enums;
using ERP.Application.DTOs.Units;

namespace ERP.Application.Features.Units.Commands
{
    public class UpdateUnitCommand : IRequest<Result<UnitDto>>
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
       public string Symbol { get; set; } = string.Empty;

        public bool Active { get; set; } = true;



    }
}
