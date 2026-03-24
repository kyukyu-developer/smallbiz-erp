
using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.ProductUnitConversion;
using ERP.Application.DTOs.Units;
using ERP.Domain.Interfaces;
using MediatR;


namespace ERP.Application.Features.ProductUnitConversion.Queries
{

    public class GetProductUnitConversionByIdQueryHandler : IRequestHandler<GetProductUnitConversionByIdQuery, Result<GetProductUnitConversionByIdDto>>
    {
        private readonly IProductUnitConversionRepository _productUnitConversionRepository;

        public GetProductUnitConversionByIdQueryHandler(IProductUnitConversionRepository productUnitConversionRepository)
        {
            _productUnitConversionRepository = productUnitConversionRepository;
        }

        public async Task<Result<GetProductUnitConversionByIdDto>> Handle(GetProductUnitConversionByIdQuery request, CancellationToken cancellationToken)
        {
            var productUnitConversion = await _productUnitConversionRepository.GetByIdAsync(request.Id);

            if (productUnitConversion == null)
            {
                return Result<GetProductUnitConversionByIdDto>.Failure($"Product Unit Conversion with ID '{request.Id}' not found");
            }

            var GetProductUnitConversionByIdDto = new GetProductUnitConversionByIdDto
            {
                Id = productUnitConversion.Id,
                ProductId = productUnitConversion.ProductId,
                FromUnitId = productUnitConversion.FromUnitId,
                ToUnitId = productUnitConversion.ToUnitId,
                Factor = productUnitConversion.Factor,
                Active = productUnitConversion.Active,
                CreatedAt = productUnitConversion.CreatedAt,
                CreatedBy = productUnitConversion.CreatedBy,
                UpdatedAt = productUnitConversion.UpdatedAt,
                UpdatedBy = productUnitConversion.UpdatedBy,
                LastAction = productUnitConversion.LastAction
            };

            return Result<GetProductUnitConversionByIdDto>.Success(GetProductUnitConversionByIdDto);
        }
    }

}
