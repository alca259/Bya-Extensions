using Microsoft.Extensions.Caching.Distributed;

namespace Bya.Cache.Threading;

public static class DistributedCaching
{
    public async static Task SetAsync<T>(this IDistributedCache distributedCache, string key, T value, CancellationToken token = default)
    {
        await distributedCache.SetAsync(key, value.ToByteArray(), token);
    }

    public async static Task SetAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        await distributedCache.SetAsync(key, value.ToByteArray(), options, token);
    }

    public async static Task<T> GetAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default) where T : class
    {
        var result = await distributedCache.GetAsync(key, token);
        return result.FromByteArray<T>();
    }
}
