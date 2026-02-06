using MediatR;
using ERP.Application.DTOs.Products;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Products.Queries
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<List<ProductDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.Products.GetAllAsync();

            // Filter by category if specified
            if (request.CategoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == request.CategoryId.Value);
            }

            // Filter by search term if specified
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                products = products.Where(p => p.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // Get all categories for mapping
            var categories = await _unitOfWork.Categories.GetAllAsync();
            var categoryDict = categories.ToDictionary(c => c.Id, c => c.Name);

            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                CategoryId = p.CategoryId,
                BaseUnitId = p.BaseUnitId,
                MinimumStock = p.MinimumStock,
                MaximumStock = p.MaximumStock,
                ReorderLevel = p.ReorderLevel,
                Barcode = p.Barcode,
                IsBatchTracked = p.IsBatchTracked,
                IsSerialTracked = p.IsSerialTracked,
                IsActive = p.IsActive,
                CategoryName = categoryDict.GetValueOrDefault(p.CategoryId, "Unknown")
            }).ToList();

            return Result<List<ProductDto>>.Success(productDtos);
        }
    }
}
