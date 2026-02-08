using Purchasing.Domain.Common;

namespace Purchasing.Domain.Entities;

public class Supplier : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? TaxId { get; set; }
    public string? ContactPerson { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
