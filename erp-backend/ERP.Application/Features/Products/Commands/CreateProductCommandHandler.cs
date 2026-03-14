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
        private readonly IUnitRepository _unitRepository;
        private readonly IUnitOfWork _unitOfWorkRepository;

        public CreateProductCommandHandler(IProductRepository productRepository, IUnitRepository unitRepository,IUnitOfWork unitOfWorkRepository)
        {
            _productRepository = productRepository;
            _unitRepository = unitRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Check if product code already exists
            var existingProduct = await _productRepository.GetByCodeAsync(request.Code);
            if (existingProduct != null)
            {
                return Result<ProductDto>.Failure("Product code already exists");
            }

            var product = new Domain.Entities.ProdItem
            {
                Code = request.Code,
                Name = request.Name,
                GroupId = request.GroupId,
                CategoryId = request.CategoryId,
                BrandId = request.BrandId,
                Description = request.Description,
                BaseUnitId = request.BaseUnitId,
                MinimumStock = request.MinimumStock,
                MaximumStock = request.MaximumStock,
                ReorderLevel = request.ReorderLevel,
                Barcode = request.Barcode,
                TrackType = request.TrackType,
                HasVariant = request.HasVariant,
                AllowNegativeStock = request.AllowNegativeStock,
                Active = request.Active
            };

            await _productRepository.AddAsync(product);
            await _unitOfWorkRepository.SaveChangesAsync();

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
