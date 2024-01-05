using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.CodeAnalysis;

namespace HackerNews.Services
{
    /// <summary>
    /// ICacheService.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="cache">the IMemoryCache.</param>
        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Set the Cache.
        /// </summary>
        /// <typeparam name="T">the value type.</typeparam>
        /// <param name="key">the Key.</param>
        /// <param name="value">the Value to set.</param>
        /// <param name="options">the MemoryCacheEntryOptions</param>
        public void Set<T>(string key, T value, MemoryCacheEntryOptions options)
        {
            _cache.Set(key, value, options);
        }

        /// <summary>
        /// Get the Cache value.
        /// </summary>
        /// <typeparam name="T">the value type.</typeparam>
        /// <param name="key">the Key.</param>
        /// <param name="value">the Value to set.</param>
        /// <returns>returns the cached data.</returns>
        public bool TryGetValue<T>(string key, out T value)
        {
            return _cache.TryGetValue(key, out value);
        }
    }
}
