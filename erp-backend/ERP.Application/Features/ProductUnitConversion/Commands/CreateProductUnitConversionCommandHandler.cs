using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.ProductUnitConversion;
using ERP.Domain.Interfaces;
using MediatR;

namespace ERP.Application.Features.ProductUnitConversion.Commands
{
    public class CreateProductUnitConversionCommandHandler : IRequestHandler<CreateProductUnitConversionCommand, Result<GetProductUnitConversionByIdDto>>
    {
        private readonly IProductUnitConversionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductUnitConversionCommandHandler(
            IProductUnitConversionRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GetProductUnitConversionByIdDto>> Handle(CreateProductUnitConversionCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByProductAndUnitsAsync(
                request.ProductId, 
                request.FromUnitId, 
                request.ToUnitId);
            
            if (existing != null)
            {
                return Result<GetProductUnitConversionByIdDto>.Failure(
                    "Product unit conversion with these units already exists");
            }

            var entity = new Domain.Entities.ProdUnitConversion
            {
                Id = Guid.NewGuid().ToString(),
                ProductId = request.ProductId,
                FromUnitId = request.FromUnitId,
                ToUnitId = request.ToUnitId,
                Factor = request.Factor,
                Active = true,
                LastAction = "CREATE",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };

            await _repository.AddAsync(entity);
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
                LastAction = entity.LastAction
            };

            return Result<GetProductUnitConversionByIdDto>.Success(dto);
        }
    }
}


