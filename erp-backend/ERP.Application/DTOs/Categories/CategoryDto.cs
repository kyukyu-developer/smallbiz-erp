namespace ERP.Application.DTOs.Categories
{
    public class CategoryDto
    {
        public string Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ParentCategoryId { get; set; }
        public bool IsActive { get; set; }
    }
}
