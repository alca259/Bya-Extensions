using Microsoft.Extensions.Caching.Distributed;

namespace Bya.Cache.Threading;

public sealed class DistributedSemaphore
{
    private readonly IDistributedCache _distributedCache;
    private static readonly SemaphoreSlim _cacheSemaphore = new(1, 1);

    public DistributedSemaphore(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async ValueTask LockAsync(string key, TimeSpan? timeout = null, CancellationToken token = default)
    {
        timeout ??= TimeSpan.FromMinutes(5);

        await _cacheSemaphore.WaitAsync(timeout.Value, token);

        long newWorker = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        Queue<DistributedSemaphoreData> workers = await GetCurrentWorkers(key, token);
        DistributedSemaphoreData lastWorker = workers.LastOrDefault();
        workers.Enqueue(new DistributedSemaphoreData(newWorker, timeout.Value));
        await _distributedCache.SetAsync(key, workers, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = timeout }, token);

        _cacheSemaphore.Release();

        if (lastWorker == null) return;

        while (true)
        {
            Thread.Sleep(500);
            workers = await GetCurrentWorkers(key, token);
            if (token.IsCancellationRequested) break;
            if (!workers.Select(s => s.WorkerID).Contains(lastWorker.WorkerID)) break;
            var expireTime = (DateTimeOffset.Now + timeout.Value).ToUnixTimeMilliseconds();
            if (newWorker >= expireTime) break;
        }
    }

    public async ValueTask ReleaseAsync(string key, CancellationToken token = default)
    {
        await _cacheSemaphore.WaitAsync(TimeSpan.FromMinutes(5), token);

        Queue<DistributedSemaphoreData> workers = await GetCurrentWorkers(key, token);
        if (workers.Count <= 0)
        {
            await _distributedCache.RemoveAsync(key, token);
            _cacheSemaphore.Release();
            return;
        }

        workers.Dequeue();
        if (workers.Count <= 0)
        {
            await _distributedCache.RemoveAsync(key, token);
            _cacheSemaphore.Release();
            return;
        }

        var timeout = workers.Peek().Timeout;
        await _distributedCache.SetAsync(key, workers, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = timeout }, token);

        _cacheSemaphore.Release();
    }

    public async ValueTask<int> CountWorkers(string key, CancellationToken token = default)
    {
        return (await GetCurrentWorkers(key, token)).Count;
    }

    private async Task<Queue<DistributedSemaphoreData>> GetCurrentWorkers(string key, CancellationToken token)
    {
        return await _distributedCache.GetAsync<Queue<DistributedSemaphoreData>>(key, token) ?? new Queue<DistributedSemaphoreData>();
    }

    private class DistributedSemaphoreData
    {
        public DistributedSemaphoreData(long workerID, TimeSpan timeout)
        {
            WorkerID = workerID;
            Timeout = timeout;
        }

        public long WorkerID { get; set; }
        public TimeSpan Timeout { get; set; }
    }
}
