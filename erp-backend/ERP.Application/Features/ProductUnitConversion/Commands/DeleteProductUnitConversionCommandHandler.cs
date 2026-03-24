using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;
using MediatR;

namespace ERP.Application.Features.ProductUnitConversion.Commands
{
    public class DeleteProductUnitConversionCommandHandler : IRequestHandler<DeleteProductUnitConversionCommand, Result<int>>
    {
        private readonly IProductUnitConversionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductUnitConversionCommandHandler(
            IProductUnitConversionRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteProductUnitConversionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
            {
                return Result<int>.Failure(
                    $"Product unit conversion with ID '{request.Id}' not found");
            }

            entity.Active = false;
            entity.LastAction = "DELETE";
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = "System";

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<int>.Success(1);
        }
    }
}
