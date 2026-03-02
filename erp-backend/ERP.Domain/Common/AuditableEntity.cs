namespace ERP.Domain.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        public bool Active { get; set; } = true;
        public string? LastAction { get; set; }                // e.g., CREATE, UPDATE, DELETE
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }


    }
}
