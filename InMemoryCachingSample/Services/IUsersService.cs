using System.Collections.Generic;
using System.Threading.Tasks;
using InMemoryCachingSample.Models;

namespace InMemoryCachingSample.Services
{
    public interface IUsersService
    {
        Task<IEnumerable<User>>GetUsers();
        Task<IEnumerable<User>>GetUsersAsync();
    }
}