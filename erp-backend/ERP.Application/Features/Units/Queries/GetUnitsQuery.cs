using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.Units;
using ERP.Application.DTOs.Warehouses;
using ERP.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.Features.Units.Queries
{

    public class GetUnitsQuery : IRequest<Result<List<UnitDto>>>
    {
        public bool? IncludeInactive { get; set; }

    }
}
