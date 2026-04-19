

namespace ERP.Domain.Interfaces;

public interface IRedisClient
{
    Task<string?> GetStringAsync(string key);
    Task SetStringAsync(string key, string value, TimeSpan expiry);
    Task DeleteAsync(string key);
    Task DeleteByPatternAsync(string pattern);
    Task<bool> AcquireLockAsync(string key, TimeSpan expiry);
    Task ReleaseLockAsync(string key);
}
