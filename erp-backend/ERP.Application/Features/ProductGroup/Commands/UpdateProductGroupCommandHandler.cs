
using MediatR;
using ERP.Application.DTOs.Warehouses;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;
using ERP.Domain.Enums;
using ERP.Application.DTOs.ProductGroup;

namespace ERP.Application.Features.ProductGroup.Commands
{
    public class UpdateProductGroupCommandHandler : IRequestHandler<UpdateProductGroupCommand, Result<ProductGroupDto>>
    {
        private readonly IProductGroupRepository _productGroupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductGroupCommandHandler(
            IProductGroupRepository productGroupRepository,
            IUnitOfWork unitOfWork)
        {
            _productGroupRepository = productGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ProductGroupDto>> Handle(UpdateProductGroupCommand request, CancellationToken cancellationToken)
        {
            // Check if productGroup exists
            var productGroup = await _productGroupRepository.GetByIdAsync(request.Id);
            if (productGroup == null)
            {
                return Result<ProductGroupDto>.Failure($"Product Group with ID '{request.Id}' not found");
            }

            // Check for duplicate product group
            var existingProductGroup = await _productGroupRepository.GetByName(request.Name);
            if (existingProductGroup != null )
            {
                return Result<ProductGroupDto>.Failure($"Product Group with name '{request.Name}' already exists ");

            }

            // Update product Group
            productGroup.Name = request.Name;
            productGroup.Description = request.Description;


            productGroup.Active = request.Active;

            productGroup.LastAction = "UPDATE";
            productGroup.UpdatedAt = DateTime.UtcNow;
            productGroup.UpdatedBy = "System"; // TODO: Get from current user context

            _productGroupRepository.Update(productGroup);
            await _unitOfWork.SaveChangesAsync();



            var ProductGroupDto = new ProductGroupDto
            {
                Id = productGroup.Id,
                Name = productGroup.Name,            
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
