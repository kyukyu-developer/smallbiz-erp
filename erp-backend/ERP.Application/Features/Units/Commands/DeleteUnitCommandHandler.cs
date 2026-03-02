

using MediatR;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Units.Commands
{
    public class DeleteUnitCommandHandler : IRequestHandler<DeleteUnitCommand, Result<bool>>
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUnitCommandHandler(
            IUnitRepository unitRepository,
            IUnitOfWork unitOfWork)
        {
            _unitRepository = unitRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
        {
            var unit = await _unitRepository.GetByIdAsync(request.Id);
            if (unit == null)
            {
                return Result<bool>.Failure($"Unit with ID '{request.Id}' not found");
            }

            // Soft delete
            unit.Active = false;
            unit.LastAction = "DELETE";
            unit.UpdatedAt = DateTime.UtcNow;
            unit.UpdatedBy = "System"; // TODO: Get from current user context

            _unitRepository.Update(unit);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
