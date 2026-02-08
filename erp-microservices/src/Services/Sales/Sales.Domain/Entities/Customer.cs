using Sales.Domain.Common;

namespace Sales.Domain.Entities;

public class Customer : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? TaxId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
