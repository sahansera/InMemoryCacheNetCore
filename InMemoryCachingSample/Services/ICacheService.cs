using InMemoryCachingSample.Models;

namespace InMemoryCachingSample.Services
{
    public interface ICacheService
    {
        User GetCachedUser();
        void ClearCache();
    }
}