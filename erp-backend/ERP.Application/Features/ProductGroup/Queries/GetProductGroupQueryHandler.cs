using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.ProductGroup;
using ERP.Application.DTOs.Units;
using ERP.Domain.Interfaces;
using MediatR;

namespace ERP.Application.Features.ProductGroup.Queries
{


    public class GetProductGroupQueryHandler : IRequestHandler<GetProductGroupQuery, Result<List<ProductGroupDto>>>
    {
        private readonly IProductGroupRepository _productGroupRepository;

        public GetProductGroupQueryHandler(IProductGroupRepository productGroupRepository)
        {
            _productGroupRepository = productGroupRepository;
        }

        public async Task<Result<List<ProductGroupDto>>> Handle(GetProductGroupQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Domain.Entities.ProdGroup> productGroups;

            productGroups = await _productGroupRepository.GetAllAsync();


            // Apply active filter
            var filteredProductGroups = productGroups
                .Select(w => new ProductGroupDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    Description = w.Description,
                    Active = w.Active,
                    CreatedAt = w.CreatedAt,
                    UpdatedAt = w.UpdatedAt,
                    CreatedBy = w.CreatedBy,
                    UpdatedBy = w.UpdatedBy,
                    LastAction = w.LastAction
                })
                .ToList();

            return Result<List<ProductGroupDto>>.Success(filteredProductGroups);
        }
    }

}
