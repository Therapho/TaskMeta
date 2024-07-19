using Microsoft.Extensions.Caching.Memory;
using TaskMeta.Shared.Interfaces;

namespace TaskMeta.Shared.Models.Providers
{
    public class CacheProvider(IMemoryCache cache) : ICacheProvider
    {

        private readonly IMemoryCache _cache = cache;

        public T? Get<T>(string key)
        {
            _cache.TryGetValue(key, out T? result);

            return result;
        }

        public void Set<T>(string key, T value)
        {
            _cache.Set(key, value);
        }

        public void Set<T>(string key, T value, int expiration)
        {
            _cache.Set(key, value, TimeSpan.FromMinutes(expiration));
        }

        public void Remove(string key)
        {
            _cache.Remove(key);            
        }
        public void Clear()
        {
            if (_cache is MemoryCache cache) cache.Clear();
        }
    }
}
