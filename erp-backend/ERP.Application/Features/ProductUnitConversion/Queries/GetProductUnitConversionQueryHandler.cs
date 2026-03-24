
using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.ProductUnitConversion;
using ERP.Application.DTOs.Units;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using MediatR;

namespace ERP.Application.Features.ProductUnitConversion.Queries
{


    public class GetProductUnitConversionQueryHandler : IRequestHandler<GetProductUnitConversionQuery, Result<List<GetProductUnitConversionDto>>>
    {
        private readonly IProductUnitConversionRepository _productUnitConversionRepository;

        public GetProductUnitConversionQueryHandler(IProductUnitConversionRepository productUnitConversionRepository)
        {
            _productUnitConversionRepository = productUnitConversionRepository;
        }

        public async Task<Result<List<GetProductUnitConversionDto>>> Handle(GetProductUnitConversionQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Domain.Entities.ProdUnitConversion> prodUnitConversions;

            prodUnitConversions = await _productUnitConversionRepository.GetAllAsync();


            // Apply active filter
            var filteredProductUnitConversions = prodUnitConversions
                .Select(w => new GetProductUnitConversionDto
                {
                    Id = w.Id,

                    ProductName = w.Product.Name,

                    FromUnitName = w.FromUnit.Name,

                    ToUnitName = w.ToUnit.Name,

                    Factor = w.Factor,


    })
                .ToList();

            return Result<List<GetProductUnitConversionDto>>.Success(filteredProductUnitConversions);
        }
    }

}
