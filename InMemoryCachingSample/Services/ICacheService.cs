using System.Collections.Generic;
using System.Threading.Tasks;
using InMemoryCachingSample.Models;

namespace InMemoryCachingSample.Services
{
    public interface ICacheService
    {
        IEnumerable<User> GetCachedUser();
        void ClearCache();
    }
}