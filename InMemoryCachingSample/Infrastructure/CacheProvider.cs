namespace InMemoryCachingSample.Infrastructure;

public interface ICacheProvider
{
    T? GetFromCache<T>(string key) where T : class;
    void SetCache<T>(string key, T value, MemoryCacheEntryOptions options) where T : class;
    void ClearCache(string key);
}

public class CacheProvider(IMemoryCache cache) : ICacheProvider
{        
    private readonly IMemoryCache _cache = cache;

  public T? GetFromCache<T>(string key) where T : class
    {
        _cache.TryGetValue(key, out T? cachedResponse);
        return cachedResponse;
    }

    public void SetCache<T>(string key, T value, MemoryCacheEntryOptions options) where T : class
    {
        _cache.Set(key, value, options);
    }

    public void ClearCache(string key)
    {
        _cache.Remove(key);
    }
}