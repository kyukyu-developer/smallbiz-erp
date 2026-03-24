using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.ProductUnitConversion;
using ERP.Domain.Interfaces;
using MediatR;

namespace ERP.Application.Features.ProductUnitConversion.Commands
{
    public class UpdateProductUnitConversionCommandHandler : IRequestHandler<UpdateProductUnitConversionCommand, Result<GetProductUnitConversionByIdDto>>
    {
        private readonly IProductUnitConversionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductUnitConversionCommandHandler(
            IProductUnitConversionRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GetProductUnitConversionByIdDto>> Handle(UpdateProductUnitConversionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
            {
                return Result<GetProductUnitConversionByIdDto>.Failure(
                    $"Product unit conversion with ID '{request.Id}' not found");
            }

            var existing = await _repository.GetByProductAndUnitsAsync(
                request.ProductId, 
                request.FromUnitId, 
                request.ToUnitId);
            
            if (existing != null && existing.Id != request.Id)
            {
                return Result<GetProductUnitConversionByIdDto>.Failure(
                    "Product unit conversion with these units already exists");
            }

            entity.ProductId = request.ProductId;
            entity.FromUnitId = request.FromUnitId;
            entity.ToUnitId = request.ToUnitId;
            entity.Factor = request.Factor;
            entity.Active = request.Active;
            entity.LastAction = "UPDATE";
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = "System";

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            var dto = new GetProductUnitConversionByIdDto
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                FromUnitId = entity.FromUnitId,
                ToUnitId = entity.ToUnitId,
                Factor = entity.Factor,
                Active = entity.Active,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedAt = entity.UpdatedAt,
                UpdatedBy = entity.UpdatedBy,
                LastAction = entity.LastAction
            };

            return Result<GetProductUnitConversionByIdDto>.Success(dto);
        }
    }
}
