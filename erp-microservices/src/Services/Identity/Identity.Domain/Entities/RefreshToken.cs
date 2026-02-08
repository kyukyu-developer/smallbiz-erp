using Identity.Domain.Common;

namespace Identity.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRevoked { get; set; }
        public int UserId { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
    }
}
