using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.Units;
using MediatR;


namespace ERP.Application.Features.Units.Queries
{
    public class GetUnitByIdQuery : IRequest<Result<UnitDto>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
