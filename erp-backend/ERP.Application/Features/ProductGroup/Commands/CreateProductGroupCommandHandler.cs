
using MediatR;
using ERP.Application.DTOs.Warehouses;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using ERP.Domain.Enums;
using ERP.Application.DTOs.Units;
using ERP.Application.DTOs.ProductGroup;

namespace ERP.Application.Features.ProductGroup.Commands
{
    public class CreateProductGroupCommandHandler : IRequestHandler<CreateProductGroupCommand, Result<ProductGroupDto>>
    {
        private readonly IProductGroupRepository _productGroupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductGroupCommandHandler(
            IProductGroupRepository productGroupRepository,
            IUnitOfWork unitOfWork)
        {
            _productGroupRepository = productGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ProductGroupDto>> Handle(CreateProductGroupCommand request, CancellationToken cancellationToken)
        {

            // Check for duplicate unit name 
            var existingProductGroup = await _productGroupRepository.GetByName(request.Name);
            if (existingProductGroup != null)
            {
                return Result<ProductGroupDto>.Failure($"Product Group with name '{request.Name}' already exists ");
            }

            var productGroup = new Domain.Entities.ProdGroup
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                Active = true,
                LastAction = "CREATE",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System" // TODO: Get from current user context
            };

            await _productGroupRepository.AddAsync(productGroup);
            await _unitOfWork.SaveChangesAsync();



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
