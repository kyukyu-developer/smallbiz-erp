
using MediatR;
using ERP.Application.DTOs.Warehouses;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;
using ERP.Application.DTOs.ProductGroup;

namespace ERP.Application.Features.ProductGroup.Queries
{
    public class GetProductGroupByIdQueryHandler : IRequestHandler<GetProductGroupByIdQuery, Result<ProductGroupDto>>
    {
        private readonly IProductGroupRepository _productGroupRepository;

        public GetProductGroupByIdQueryHandler(IProductGroupRepository productGroupRepository)
        {
            _productGroupRepository = productGroupRepository;
        }

        public async Task<Result<ProductGroupDto>> Handle(GetProductGroupByIdQuery request, CancellationToken cancellationToken)
        {
            var productGroup = await _productGroupRepository.GetByIdAsync(request.Id);

            if (productGroup == null)
            {
                return Result<ProductGroupDto>.Failure($"Product Group with ID '{request.Id}' not found");
            }

   
            var ProductGroupDto = new ProductGroupDto
            {
                Id = productGroup.Id,
                Name = productGroup.Name,
                Description = productGroup.Description,
                Active = productGroup.Active,
              
                CreatedAt = productGroup.CreatedAt,
                UpdatedAt = productGroup.UpdatedAt,
                CreatedBy = productGroup.CreatedBy,
                UpdatedBy = productGroup.UpdatedBy,
                LastAction = productGroup.LastAction
            };

            return Result<ProductGroupDto>.Success(ProductGroupDto);
        }
    }
}
