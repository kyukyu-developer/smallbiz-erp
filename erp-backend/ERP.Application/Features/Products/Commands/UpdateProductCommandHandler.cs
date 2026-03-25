using MediatR;
using ERP.Application.DTOs.Products;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Products.Commands
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<ProductDto>>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null)
            {
                return Result<ProductDto>.Failure("Product not found");
            }

            // Check if code is being changed and if new code already exists
            if (product.Code != request.Code)
            {
                var existingProduct = await _productRepository.GetByCodeAsync(request.Code);
                if (existingProduct != null)
                {
                    return Result<ProductDto>.Failure("Product code already exists");
                }
            }

            product.Code = request.Code;
            product.Name = request.Name;
            product.GroupId = request.GroupId;
            product.CategoryId = request.CategoryId;
            product.BrandId = request.BrandId;
            product.Description = request.Description;
            product.BaseUnitId = request.BaseUnitId;
            product.MinimumStock = request.MinimumStock;
            product.MaximumStock = request.MaximumStock;
            product.ReorderLevel = request.ReorderLevel;
            product.Barcode = request.Barcode;
            product.TrackType = request.TrackType;
            product.HasVariant = request.HasVariant;
            product.AllowNegativeStock = request.AllowNegativeStock;
            product.Active = request.Active;
            product.LastAction = "UPDATE";

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();

            var productDto = new ProductDto
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                GroupId = product.GroupId,
                CategoryId = product.CategoryId,
                BrandId = product.BrandId,
                Description = product.Description,
                BaseUnitId = product.BaseUnitId,
                MinimumStock = product.MinimumStock,
                MaximumStock = product.MaximumStock,
                ReorderLevel = product.ReorderLevel,
                Barcode = product.Barcode,
                TrackType = product.TrackType,
                HasVariant = product.HasVariant,
                AllowNegativeStock = product.AllowNegativeStock,
                Active = product.Active
            };

            return Result<ProductDto>.Success(productDto);
        }
    }
}
