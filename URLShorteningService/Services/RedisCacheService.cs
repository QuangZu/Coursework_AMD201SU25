using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

public class RedisCacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetCacheAsync<T>(string key)
    {
        var cached = await _cache.GetStringAsync(key);
        return cached == null ? default : JsonSerializer.Deserialize<T>(cached);
    }

    public async Task SetCacheAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(10)
        };
        var json = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key, json, options);
    }
}
