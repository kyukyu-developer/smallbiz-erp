

using MediatR;
using ERP.Application.DTOs.Warehouses;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;
using ERP.Domain.Enums;
using ERP.Application.DTOs.Units;

namespace ERP.Application.Features.Units.Commands
{
    public class UpdateUnitCommandHandler : IRequestHandler<UpdateUnitCommand, Result<UnitDto>>
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUnitCommandHandler(
            IUnitRepository unitRepository,
            IUnitOfWork unitOfWork)
        {
            _unitRepository = unitRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UnitDto>> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
        {
            // Check if unit exists
            var unit = await _unitRepository.GetByIdAsync(request.Id);
            if (unit == null)
            {
                return Result<UnitDto>.Failure($"Unit with ID '{request.Id}' not found");
            }

        
            // Check for duplicate unit name 
            var existingUnit = await _unitRepository.GetByName(request.Name);
            if (existingUnit != null && existingUnit.Id != request.Id)
            {
                return Result<UnitDto>.Failure($"Unit with name '{request.Name}' already exists ");
            }

            // Update unit
            unit.Name = request.Name;
            unit.Symbol = request.Symbol;
            unit.Active = request.Active;
            unit.LastAction = "UPDATE";
            unit.UpdatedAt = DateTime.UtcNow;
            unit.UpdatedBy = "System"; // TODO: Get from current user context
            _unitRepository.Update(unit);
            await _unitOfWork.SaveChangesAsync();



            var UnitDto = new UnitDto
            {
                Id = unit.Id,
                Name = unit.Name,
                Symbol = unit.Symbol,
                Active = unit.Active,
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
