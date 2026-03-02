using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.Units;
using ERP.Domain.Interfaces;
using MediatR;

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
            
                units = await _unitRepository.GetAllAsync();


            // Apply active filter
            var filteredUnits = units
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
