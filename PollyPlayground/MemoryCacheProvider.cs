using Polly.Caching;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace PollyPlayground
{
    public class MemoryCacheProvider : IAsyncCacheProvider
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheProvider()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        public async Task<T> GetAsync<T>(string key)
        {
            // Try to get the item from the cache
            if (_memoryCache.TryGetValue(key, out T cachedValue))
            {
                return await Task.FromResult(cachedValue);
            }
            return default;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan ttl)
        {
            // Set the item in the cache with a TTL (Time to Live)
            _memoryCache.Set(key, value, ttl);
            await Task.CompletedTask;
        }

        public async Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            await Task.CompletedTask;
        }

        // Implementing TryGetAsync to return a tuple (bool, object?)
        public async Task<(bool, object?)> TryGetAsync(string key, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            // Try to get the item from the cache
            if (_memoryCache.TryGetValue(key, out object cachedValue))
            {
                return await Task.FromResult((true, cachedValue));
            }

            return await Task.FromResult((false, cachedValue));
        }

        // Implementing PutAsync to store data in the cache
        public async Task PutAsync(string key, object? value, Ttl ttl, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            // Convert the Polly Ttl (Time to Live) to a TimeSpan for MemoryCache
            var expiration = ttl.Timespan;
            _memoryCache.Set(key, value, expiration);
            await Task.CompletedTask;
        }
        
    }
}