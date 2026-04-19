using ERP.Domain.Interfaces;
using StackExchange.Redis;

namespace ERP.Infrastructure.Services;

public class RedisClient : IRedisClient
{
    private readonly IConnectionMultiplexer _connection;
    private readonly IDatabase _database;

    public RedisClient(IConnectionMultiplexer connection)
    {
        _connection = connection;
        _database = connection.GetDatabase();
    }

    public async Task<string?> GetStringAsync(string key)
    {
        var value = await _database.StringGetAsync(key);
        if (value == RedisValue.Null)
            return null;
        return (string)value!;
    }

    public Task SetStringAsync(string key, string value, TimeSpan expiry)
    {
        return _database.StringSetAsync(key, value, expiry);
    }

    public Task DeleteAsync(string key)
    {
        return _database.KeyDeleteAsync(key);
    }

    public async Task DeleteByPatternAsync(string pattern)
    {
        var server = _connection.GetServer(_connection.GetEndPoints().First());
        var keys = server.Keys(pattern: $"*{pattern}*").ToArray();

        if (keys.Length > 0)
            await _database.KeyDeleteAsync(keys);
    }

    public async Task<bool> AcquireLockAsync(string key, TimeSpan expiry)
    {
        return await _database.StringSetAsync(key, "locked", expiry, when: When.NotExists);
    }

    public Task ReleaseLockAsync(string key)
    {
        return _database.KeyDeleteAsync(key);
    }
}
