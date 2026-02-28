using MediatR;
using ERP.Application.DTOs.Products;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Products.Commands
{
    public class UpdateProductCommand : IRequest<Result<ProductDto>>
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public string BaseUnitId { get; set; } = string.Empty;
        public decimal? MinimumStock { get; set; }
        public decimal? MaximumStock { get; set; }
        public decimal? ReorderLevel { get; set; }
        public string? Barcode { get; set; }
        public bool IsBatchTracked { get; set; }
        public bool IsSerialTracked { get; set; }
        public bool IsActive { get; set; }
    }
}
