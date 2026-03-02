using ERP.Domain.Common;

namespace ERP.Domain.Entities
{
    public class User : AuditableEntity
    {

        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // Navigation properties
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
