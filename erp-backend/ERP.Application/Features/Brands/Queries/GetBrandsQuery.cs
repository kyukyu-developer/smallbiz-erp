using ERP.Application.DTOs.Brands;
using ERP.Application.DTOs.Common;
using MediatR;


namespace ERP.Application.Features.Brands.Queries
{


    public class GetBrandsQuery : IRequest<Result<List<BrandDto>>>
    {

    }
}
