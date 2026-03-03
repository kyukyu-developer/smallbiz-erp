

using MediatR;
using ERP.Application.DTOs.Warehouses;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using ERP.Domain.Enums;
using ERP.Application.DTOs.Units;

namespace ERP.Application.Features.Units.Commands
{
    public class CreateUnitCommandHandler : IRequestHandler<CreateUnitCommand, Result<UnitDto>>
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUnitCommandHandler(
            IUnitRepository unitRepository,
            IUnitOfWork unitOfWork)
        {
            _unitRepository = unitRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UnitDto>> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
        {
         
            // Check for duplicate unit name 
            var existingUnit = await _unitRepository.GetByName(request.Name);
            if (existingUnit != null)
            {
                return Result<UnitDto>.Failure($"Unit with name '{request.Name}' already exists ");
            }

            var unit = new Domain.Entities.Units
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Symbol = request.Symbol,
                Active = true,
                LastAction = "CREATE",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System" // TODO: Get from current user context
            };

            await _unitRepository.AddAsync(unit);
            await _unitOfWork.SaveChangesAsync();

      

            var unitDto = new UnitDto
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

            return Result<UnitDto>.Success(unitDto);
        }
    }
}
