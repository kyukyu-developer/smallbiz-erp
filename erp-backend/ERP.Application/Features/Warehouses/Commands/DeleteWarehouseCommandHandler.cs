using MediatR;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Warehouses.Commands
{
    public class DeleteWarehouseCommandHandler : IRequestHandler<DeleteWarehouseCommand, Result<bool>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteWarehouseCommandHandler(
            IWarehouseRepository warehouseRepository,
            IUnitOfWork unitOfWork)
        {
            _warehouseRepository = warehouseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouse = await _warehouseRepository.GetByIdAsync(request.Id);
            if (warehouse == null)
            {
                return Result<bool>.Failure($"Warehouse with ID '{request.Id}' not found");
            }

            // Block deletion if warehouse has active child warehouses
            var activeChildren = await _warehouseRepository.GetChildWarehousesAsync(request.Id);
            if (activeChildren.Any())
            {
                return Result<bool>.Failure(
                    "Cannot delete warehouse because it has active child warehouses. " +
                    "Please delete or reassign child warehouses first.");
            }

            // Soft delete
            warehouse.Active = false;
            warehouse.LastAction = "DELETE";
            warehouse.UpdatedAt = DateTime.UtcNow;
            warehouse.UpdatedBy = "System"; // TODO: Get from current user context

            _warehouseRepository.Update(warehouse);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
