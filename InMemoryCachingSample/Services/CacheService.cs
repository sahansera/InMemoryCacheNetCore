using System.Collections.Generic;
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

        public IEnumerable<User> GetCachedUser()
        {
            return _cacheProvider.GetFromCache<IEnumerable<User>>(CacheKeys.Users);
        }

        public void ClearCache()
        {
            _cacheProvider.ClearCache(CacheKeys.Users);
        }
    }
}