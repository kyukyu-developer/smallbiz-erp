using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.Units;
using ERP.Application.DTOs.Warehouses;
using ERP.Application.Features.Warehouses.Queries;
using ERP.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.Features.Units.Queries
{


    public class GetUnitsQueryHandler : IRequestHandler<GetUnitsQuery, Result<List<UnitDto>>>
    {
        private readonly IUnitRepository _unitRepository;

        public GetUnitsQueryHandler(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public async Task<Result<List<UnitDto>>> Handle(GetUnitsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Domain.Entities.Unit> units;

            // Filter by branch type if specified
        
            
                units = await _unitRepository.GetAllAsync();


            // Apply active filter
            var filteredUnits = units
                .Where(w => (request.IncludeInactive ?? false) || w.Active)
                .Select(w => new UnitDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    Symbol = w.Symbol,
                    Active = w.Active,
                    CreatedOn = w.CreatedAt,
                    ModifiedOn = w.UpdatedAt,
                    CreatedBy = w.CreatedBy,
                    ModifiedBy = w.UpdatedBy,
                    LastAction = w.LastAction
                })
                .ToList();

            return Result<List<UnitDto>>.Success(filteredUnits);
        }
    }

}
