using InMemoryCachingSample.Infrastructure;
using InMemoryCachingSample.Models;
using InMemoryCachingSample.Utils;

namespace InMemoryCachingSample.Services
{
    public class CacheService : ICacheService
    {
        private readonly ICacheProvider _cacheProvider;

        public CacheService(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public User GetCachedUser()
        {
            throw new System.NotImplementedException();
        }

        public void ClearCache()
        {
            _cacheProvider.ClearCache(CacheKeys.Users);
        }
    }
}