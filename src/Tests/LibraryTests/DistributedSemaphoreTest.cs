using Bya.Cache.Threading;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

[assembly: Parallelize(Workers = 2, Scope = ExecutionScope.MethodLevel)]
namespace LibraryTests;

[TestClass]
public class DistributedSemaphoreTest
{
    private static IDistributedCache GetDistributedCache()
    {
        var opts = Options.Create(new MemoryDistributedCacheOptions());
        IDistributedCache cache = new MemoryDistributedCache(opts);
        return cache;
    }

    private readonly DistributedSemaphore _distributeSemaphore = new(GetDistributedCache());
    private const string KEY = "LOCK1";

    [TestMethod]
    public async Task Simple()
    {
        Assert.AreEqual(0, await _distributeSemaphore.CountWorkers(KEY));
        await _distributeSemaphore.LockAsync(KEY, TimeSpan.FromMinutes(5));
        Assert.AreEqual(1, await _distributeSemaphore.CountWorkers(KEY));
        await _distributeSemaphore.ReleaseAsync(KEY);
        Assert.AreEqual(0, await _distributeSemaphore.CountWorkers(KEY));
    }

    [TestMethod]
    public async Task Parallel()
    {
        Assert.AreEqual(0, await _distributeSemaphore.CountWorkers(KEY));
        await _distributeSemaphore.LockAsync(KEY, TimeSpan.FromMinutes(5));
        Assert.AreEqual(1, await _distributeSemaphore.CountWorkers(KEY));

        var parallelTask = Task.Factory.StartNew(async () => await ParallelUse(10));

        await Task.Delay(TimeSpan.FromSeconds(2));
        Assert.AreEqual(2, await _distributeSemaphore.CountWorkers(KEY));

        await Task.Delay(TimeSpan.FromSeconds(5)); // SHORT ASYNC TASK...
        await _distributeSemaphore.ReleaseAsync(KEY);
        Assert.AreEqual(1, await _distributeSemaphore.CountWorkers(KEY));

        await parallelTask.Result;
        Assert.AreEqual(0, await _distributeSemaphore.CountWorkers(KEY));
    }

    private async Task ParallelUse(int delaySeconds)
    {
        await _distributeSemaphore.LockAsync(KEY, TimeSpan.FromMinutes(5));
        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
        await _distributeSemaphore.ReleaseAsync(KEY);
    }
}
