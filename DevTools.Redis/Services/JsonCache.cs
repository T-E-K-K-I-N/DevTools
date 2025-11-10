using DevTools.Redis.Abstractions;
using DevTools.Redis.Options;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace DevTools.Redis.Services;

/// <inheridoc />
internal sealed class JsonCache<TOptions> : IJsonCache
    where TOptions : class, IRedisOptions
{
    private readonly IRedisOptions _options;
    private readonly IDistributedCache _distributedCache;

    /// <inheridoc />
    public JsonCache(IRedisOptions options, IDistributedCache distributedCache)
    {
        _options = options;
        _distributedCache = distributedCache;
    }

    /// <inheridoc />
    public async Task<TValue?> GetAsync<TValue>(string key, CancellationToken cancellationToken = default)
    {
        var json = await _distributedCache.GetStringAsync(key, cancellationToken);
        return json is null ? default : JsonSerializer.Deserialize<TValue>(json);
    }

    /// <inheridoc />
    public async Task<TValue> GetOrCreateAsync<TValue>(
        string key, 
        Func<ValueTask<TValue>> factory, 
        TimeSpan? expiration, 
        CancellationToken cancellationToken = default)
    {
        var existingValue = await GetAsync<TValue>(key, cancellationToken);

        if (existingValue != null)
        {
            return existingValue;
        }

        var newValue = await factory();
        await SetAsync(
            key, 
            newValue, 
            expiration, 
            cancellationToken);
        return newValue;
    }

    /// <inheridoc />
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }

    /// <inheridoc />
    public async Task SetAsync<TValue>(
        string key, 
        TValue value, 
        TimeSpan? expiration, 
        CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(value);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ??
            (_options.DefaultExpiratiobSeconds.HasValue ?
            TimeSpan.FromSeconds(_options.DefaultExpiratiobSeconds.Value) : null)
        };

        await _distributedCache.SetStringAsync(
            key, 
            json, 
            options, 
            cancellationToken);
    }
}
