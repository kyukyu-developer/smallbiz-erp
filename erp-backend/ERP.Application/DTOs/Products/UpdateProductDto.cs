namespace ERP.Application.DTOs.Products
{
    public class UpdateProductDto
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? GroupId { get; set; }
        public string? CategoryId { get; set; }
        public string? BrandId { get; set; }
        public string? Description { get; set; }
        public string BaseUnitId { get; set; } = string.Empty;
        public decimal? MinimumStock { get; set; }
        public decimal? MaximumStock { get; set; }
        public decimal? ReorderLevel { get; set; }
        public string? Barcode { get; set; }
        public int TrackType { get; set; }
        public bool HasVariant { get; set; }
        public bool? AllowNegativeStock { get; set; }
        public bool Active { get; set; }
    }
}
