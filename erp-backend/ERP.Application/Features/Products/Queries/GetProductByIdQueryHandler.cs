using MediatR;
using ERP.Application.DTOs.Products;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Products.Queries
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null)
            {
                return Result<ProductDto>.Failure("Product not found");
            }

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
