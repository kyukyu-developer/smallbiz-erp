using ERP.Domain.Common;

namespace ERP.Domain.Entities
{
    public class Customer : AuditableEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? TaxNumber { get; set; }
        public decimal? CreditLimit { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}
