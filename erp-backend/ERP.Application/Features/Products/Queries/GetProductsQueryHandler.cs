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
            if (!String.IsNullOrEmpty(request.CategoryId))
            {
                products = products.Where(p => p.CategoryId == request.CategoryId);
            }

            // Filter by search term if specified
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                products = products.Where(p => p.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                GroupId = p.GroupId,
                CategoryId = p.CategoryId,
                BrandId = p.BrandId,
                Description = p.Description,
                BaseUnitId = p.BaseUnitId,
                MinimumStock = p.MinimumStock,
                MaximumStock = p.MaximumStock,
                ReorderLevel = p.ReorderLevel,
                Barcode = p.Barcode,
                TrackType = p.TrackType,
                HasVariant = p.HasVariant,
                AllowNegativeStock = p.AllowNegativeStock,
                Active = p.Active
            }).ToList();

            return Result<List<ProductDto>>.Success(productDtos);
        }
    }
}
