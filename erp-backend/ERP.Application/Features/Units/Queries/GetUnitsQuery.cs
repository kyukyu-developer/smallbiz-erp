using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.Units;
using MediatR;

namespace ERP.Application.Features.Units.Queries
{

    public class GetUnitsQuery : IRequest<Result<List<UnitDto>>>
    {

    }
}
