using InMemoryCachingSample.Infrastructure;
using InMemoryCachingSample.Models;

namespace InMemoryCachingSample.Services;

public interface IUsersService
{
    Task<IEnumerable<User>> GetUsersAsync();
}

public class UsersService(IHttpClient httpClient) : IUsersService
{
    private readonly IHttpClient _httpClient = httpClient;

  public Task<IEnumerable<User>> GetUsersAsync()
    {
        return _httpClient.Get();
    }
}