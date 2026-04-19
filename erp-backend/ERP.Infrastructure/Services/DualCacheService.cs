using ERP.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

namespace ERP.Infrastructure.Services;

public class DualCacheService : ICacheService
{
    private readonly IRedisClient _redis;
    private readonly IMemoryCache _memory;
    private readonly ILogger<DualCacheService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public DualCacheService(
        IRedisClient redis,
        IMemoryCache memory,
        ILogger<DualCacheService> logger)
    {
        _redis = redis;
        _memory = memory;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        return GetFromDualCacheAsync<T>(key, ct);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken ct = default)
    {
        return SetToDualCacheAsync(key, value, expiry, ct);
    }

    public Task RemoveAsync(string key, CancellationToken ct = default)
    {
        return RemoveFromDualCacheAsync(key, ct);
    }

    public Task RemoveByPatternAsync(string pattern, CancellationToken ct = default)
    {
        return InvalidateByPatternAsync(pattern, ct);
    }

    public async Task<T> GetOrSetAsync<T>(
        string key,
        Func<Task<T>> factory,
        TimeSpan? expiry = null,
        CancellationToken ct = default)
    {
        var cached = await GetFromDualCacheAsync<T>(key, ct);
        if (cached != null)
            return cached;

        var result = await factory();
        if (result != null)
            await SetToDualCacheAsync(key, result, expiry, ct);

        return result;
    }

    public async Task<T> GetFromDatabaseAsync<T>(
        string key,
        Func<Task<T>> factory,
        TimeSpan? expiry = null,
        CancellationToken ct = default)
    {
        var result = await factory();
        if (result != null)
            await SetToDualCacheAsync(key, result, expiry, ct);

        return result;
    }

    public Task InvalidateCacheAsync(string key, CancellationToken ct = default)
    {
        return RemoveFromDualCacheAsync(key, ct);
    }

    public async Task InvalidateByPatternAsync(string pattern, CancellationToken ct = default)
    {
        await _redis.DeleteByPatternAsync(pattern);
        _logger.LogDebug("Pattern {Pattern} invalidated in Redis", pattern);
    }

    public async Task<T?> GetFromDualCacheAsync<T>(string key, CancellationToken ct = default)
    {
        // Step 1: Check Memory Cache (L1)
        if (_memory.TryGetValue(key, out T? memoryValue))
        {
            _logger.LogDebug("MEMORY CACHE HIT: {Key}", key);
            return memoryValue;
        }

        // Step 2: Check Redis Cache (L2)
        var redisValue = await _redis.GetStringAsync(key);
        if (!string.IsNullOrEmpty(redisValue))
        {
            _logger.LogDebug("REDIS CACHE HIT: {Key}", key);

            var deserialized = JsonSerializer.Deserialize<T>(redisValue, _jsonOptions);

            // Populate Memory Cache
            var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            _memory.Set(key, deserialized, options);

            return deserialized;
        }

        _logger.LogDebug("CACHE MISS: {Key}", key);
        return default;
    }

    public async Task SetToDualCacheAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken ct = default)
    {
        var expiryTime = expiry ?? TimeSpan.FromMinutes(10);
        var json = JsonSerializer.Serialize(value, _jsonOptions);

        // Step 1: Update Redis First (L2)
        await _redis.SetStringAsync(key, json, expiryTime);
        _logger.LogDebug("REDIS CACHE SET: {Key}", key);

        // Step 2: Update Memory Second (L1)
        var memoryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
        _memory.Set(key, value, memoryOptions);
        _logger.LogDebug("MEMORY CACHE SET: {Key}", key);
    }

    public async Task RemoveFromDualCacheAsync(string key, CancellationToken ct = default)
    {
        // Step 1: Remove Redis First
        await _redis.DeleteAsync(key);
        _logger.LogDebug("REDIS CACHE REMOVED: {Key}", key);

        // Step 2: Remove Memory Second
        _memory.Remove(key);
        _logger.LogDebug("MEMORY CACHE REMOVED: {Key}", key);
    }
}
