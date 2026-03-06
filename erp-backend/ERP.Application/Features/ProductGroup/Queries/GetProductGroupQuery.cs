
using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.ProductGroup;
using ERP.Application.DTOs.Units;
using MediatR;

namespace ERP.Application.Features.ProductGroup.Queries
{

    public class GetProductGroupQuery : IRequest<Result<List<ProductGroupDto>>>
    {

    }
}
