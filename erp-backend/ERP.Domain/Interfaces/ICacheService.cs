namespace ERP.Domain.Interfaces;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken ct = default);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken ct = default);
    Task RemoveAsync(string key, CancellationToken ct = default);
    Task RemoveByPatternAsync(string pattern, CancellationToken ct = default);

    Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null, CancellationToken ct = default);
    Task<T> GetFromDatabaseAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null, CancellationToken ct = default);
    Task InvalidateCacheAsync(string key, CancellationToken ct = default);
    Task InvalidateByPatternAsync(string pattern, CancellationToken ct = default);

    Task<T?> GetFromDualCacheAsync<T>(string key, CancellationToken ct = default);
    Task SetToDualCacheAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken ct = default);
    Task RemoveFromDualCacheAsync(string key, CancellationToken ct = default);
}
