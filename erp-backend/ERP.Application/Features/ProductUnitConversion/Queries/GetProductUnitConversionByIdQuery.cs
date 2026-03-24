
using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.ProductUnitConversion;
using ERP.Application.DTOs.Units;
using MediatR;


namespace ERP.Application.Features.ProductUnitConversion.Queries
{
    public class GetProductUnitConversionByIdQuery : IRequest<Result<GetProductUnitConversionByIdDto>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
