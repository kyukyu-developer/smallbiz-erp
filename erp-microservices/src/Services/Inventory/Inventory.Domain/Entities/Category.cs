using Inventory.Domain.Common;

namespace Inventory.Domain.Entities;

public class Category : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ParentCategoryId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Category? ParentCategory { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
