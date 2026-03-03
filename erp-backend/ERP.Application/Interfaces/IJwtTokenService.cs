using ERP.Domain.Entities;

namespace ERP.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(Users user);
        string GenerateRefreshToken();
        bool ValidateToken(string token);
        int? GetUserIdFromToken(string token);
    }
}
