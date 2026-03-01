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

    public class GetUnitByIdQueryHandler : IRequestHandler<GetUnitByIdQuery, Result<UnitDto>>
    {
        private readonly IUnitRepository _unitRepository;

        public GetUnitByIdQueryHandler(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public async Task<Result<UnitDto>> Handle(GetUnitByIdQuery request, CancellationToken cancellationToken)
        {
            var unit = await _unitRepository.GetByIdAsync(request.Id);

            if (unit == null)
            {
                return Result<UnitDto>.Failure($"Unit with ID '{request.Id}' not found");
            }
     
            var UnitDto = new UnitDto
            {
                Id = unit.Id,
                Name = unit.Name,
                Symbol = unit.Symbol,
                CreatedOn = unit.CreatedAt,
                ModifiedOn = unit.UpdatedAt,
                CreatedBy = unit.CreatedBy,
                ModifiedBy = unit.UpdatedBy,
                LastAction = unit.LastAction
            };

            return Result<UnitDto>.Success(UnitDto);
        }
    }

}
