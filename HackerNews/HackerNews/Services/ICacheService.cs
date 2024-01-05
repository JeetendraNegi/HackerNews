using Microsoft.Extensions.Caching.Memory;

namespace HackerNews.Services
{
    public interface ICacheService
    {
        void Set<T>(string key, T value, MemoryCacheEntryOptions options);
        bool TryGetValue<T>(string key, out T value);
    }
}
