using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InMemoryCachingSample.Infrastructure;
using InMemoryCachingSample.Models;
using InMemoryCachingSample.Utils;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCachingSample.Services
{
    public class CachedUserService : IUsersService
    {
        
        private readonly UsersService _usersService;
        private readonly ICacheProvider _cacheProvider;

        private static readonly SemaphoreSlim GetUsersSemaphore = new SemaphoreSlim(1, 1);

        public CachedUserService(UsersService usersService, ICacheProvider cacheProvider)
        {
            _usersService = usersService;
            _cacheProvider = cacheProvider;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await GetCachedResponse(CacheKeys.Users, () => _usersService.GetUsersAsync());
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await GetCachedResponse(CacheKeys.Users, GetUsersSemaphore, () => _usersService.GetUsersAsync());
        }

        private async Task<IEnumerable<User>> GetCachedResponse(string cacheKey, Func<Task<IEnumerable<User>>> func)
        {
            var users = _cacheProvider.GetFromCache<IEnumerable<User>>(cacheKey);
            if (users != null) return users;
            
            // Key not in cache, so get data.
            users = await func();
            
            // Set cache options.
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration(TimeSpan.FromSeconds(10)); 
            
            _cacheProvider.SetCache(cacheKey, users, cacheEntryOptions);

            return users;
        }
        
        private async Task<IEnumerable<User>> GetCachedResponse(string cacheKey, SemaphoreSlim semaphore, Func<Task<IEnumerable<User>>> func)
        {
            var users = _cacheProvider.GetFromCache<IEnumerable<User>>(cacheKey);

            if (users != null) return users;
            try
            {
                await semaphore.WaitAsync();
                users = _cacheProvider.GetFromCache<IEnumerable<User>>(cacheKey); // Recheck to make sure it didn't populate before entering semaphore
                if (users != null)
                {
                    return users;
                }
                users = await func();
                
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10)); 
                
                _cacheProvider.SetCache(cacheKey, users, cacheEntryOptions);
            }
            finally
            {
                semaphore.Release();
            }

            return users;
        }
    }
}