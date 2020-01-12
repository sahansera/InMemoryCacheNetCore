using System.Collections.Generic;
using System.Threading.Tasks;
using InMemoryCachingSample.Infrastructure;
using InMemoryCachingSample.Models;

namespace InMemoryCachingSample.Services
{

    public class UsersService : IUsersService
    {
        private readonly IHttpClient _httpClient;

        public UsersService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public Task<IEnumerable<User>> GetUsersAsync()
        {
            return _httpClient.Get();
        }
    }
}