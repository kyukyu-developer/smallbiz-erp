

using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.ProductUnitConversion;
using ERP.Domain.Enums;
using MediatR;

namespace ERP.Application.Features.ProductUnitConversion.Queries
{
    public class GetProductUnitConversionQuery : IRequest<Result<List<GetProductUnitConversionDto>>>
    {
     
    }
}
