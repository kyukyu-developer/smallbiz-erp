using MediatR;
using ERP.Application.DTOs.Products;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Products.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductDto>>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Check if product code already exists
            var existingProduct = await _productRepository.GetByCodeAsync(request.Code);
            if (existingProduct != null)
            {
                return Result<ProductDto>.Failure("Product code already exists");
            }

            var product = new Product
            {
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                BaseUnitId = request.BaseUnitId,
                MinimumStock = request.MinimumStock,
                MaximumStock = request.MaximumStock,
                ReorderLevel = request.ReorderLevel,
                Barcode = request.Barcode,
                IsBatchTracked = request.IsBatchTracked,
                IsSerialTracked = request.IsSerialTracked,
                IsActive = request.IsActive
            };

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            var productDto = new ProductDto
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                BaseUnitId = product.BaseUnitId,
                MinimumStock = product.MinimumStock,
                MaximumStock = product.MaximumStock,
                ReorderLevel = product.ReorderLevel,
                Barcode = product.Barcode,
                IsBatchTracked = product.IsBatchTracked,
                IsSerialTracked = product.IsSerialTracked,
                IsActive = product.IsActive
            };

            return Result<ProductDto>.Success(productDto);
        }
    }
}
