using InMemoryCachingSample.Infrastructure;
using InMemoryCachingSample.Models;
using InMemoryCachingSample.Utils;

namespace InMemoryCachingSample.Services;

public interface ICacheService
{
    IEnumerable<User>? GetCachedUser();
    void ClearCache();
}

public class CacheService(ICacheProvider cacheProvider) : ICacheService
{
    private readonly ICacheProvider _cacheProvider = cacheProvider;

  public IEnumerable<User>? GetCachedUser()
    {
        return _cacheProvider.GetFromCache<IEnumerable<User>>(CacheKeys.Users);
    }

    public void ClearCache()
    {
        _cacheProvider.ClearCache(CacheKeys.Users);
    }
}