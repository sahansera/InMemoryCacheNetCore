// With implicit usings, we only need specific namespaces not covered by default
using InMemoryCachingSample.Infrastructure;
using InMemoryCachingSample.Models;
using InMemoryCachingSample.Utils;

namespace InMemoryCachingSample.Services;

public class CachedUserService(IUsersService usersService, ICacheProvider cacheProvider) : IUsersService
{
    private readonly IUsersService _usersService = usersService;
    private readonly ICacheProvider _cacheProvider = cacheProvider;
    private const int CacheTTLInSeconds = 10;
    private readonly MemoryCacheEntryOptions _cacheEntryOptions = new()
    {
        SlidingExpiration = TimeSpan.FromSeconds(CacheTTLInSeconds)
    };

    private static readonly SemaphoreSlim GetUsersSemaphore = new(1, 1);

  public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await GetCachedResponse(CacheKeys.Users, GetUsersSemaphore, _usersService.GetUsersAsync);
    }
    
    private async Task<IEnumerable<User>> GetCachedResponse(string cacheKey, SemaphoreSlim semaphore, Func<Task<IEnumerable<User>>> func)
    {
        var users = _cacheProvider.GetFromCache<IEnumerable<User>>(cacheKey);
        if (users != null) return users;

        try
        {
            await semaphore.WaitAsync();
            
            // Recheck to make sure it didn't populate before entering semaphore
            users = _cacheProvider.GetFromCache<IEnumerable<User>>(cacheKey);
            if (users != null) return users;

            users = await func();
            
            _cacheProvider.SetCache(cacheKey, users, _cacheEntryOptions);
        }
        finally
        {
            semaphore.Release();
        }

        return users;
    }
}