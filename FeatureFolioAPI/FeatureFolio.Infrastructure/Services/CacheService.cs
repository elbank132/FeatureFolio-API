using FeatureFolio.Application.Interfaces;
using FeatureFolio.Infrastructure.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FeatureFolio.Infrastructure.Services;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly RedisOptions _redisOptions;

    public CacheService(IDistributedCache cache, IOptions<RedisOptions> options)
    {
        _cache = cache;
        _redisOptions = options.Value;
    }

    public async Task SetAsync<T>(string key, T data)
    {
        var TTL = new TimeSpan(0, _redisOptions.TTL, 0);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TTL
        };

        var jsonData = JsonSerializer.Serialize(data);
        await _cache.SetStringAsync(key, jsonData, options);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var jsonData = await _cache.GetStringAsync(key);

        if (string.IsNullOrEmpty(jsonData))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(jsonData);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}
