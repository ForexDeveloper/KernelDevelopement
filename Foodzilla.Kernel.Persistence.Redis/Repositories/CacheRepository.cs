using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Foodzilla.Kernel.Contract.CacheRepositories;

namespace Foodzilla.Kernel.Persistence.Redis.Repositories;

public abstract class CacheRepository : ICacheRepository
{
    private readonly IDistributedCache _distributedCache;

    protected CacheRepository(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task StoreAsync<TValue>(string key, TValue value)
    {
        await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value));
    }

    public async Task<TValue?> GetAsync<TValue>(string key)
    {
        var value = await _distributedCache.GetStringAsync(key);

        return value != null ? JsonSerializer.Deserialize<TValue>(value) : default(TValue);
    }
}