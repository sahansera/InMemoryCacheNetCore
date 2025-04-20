using InMemoryCachingSample.Models;

namespace InMemoryCachingSample.Infrastructure;

public class UserResponse
{
    public User[] Data { get; set; } = [];
}

public interface IHttpClient
{
    Task<IEnumerable<User>> Get();
}

public class HttpClient(IHttpClientFactory clientFactory) : IHttpClient
{
    private const string API_URL = "https://reqres.in/api/users";
    
    private readonly IHttpClientFactory _clientFactory = clientFactory;

  public async Task<IEnumerable<User>> Get()
    {
        var client = _clientFactory.CreateClient();

        try
        {
            var usersResponse = await client.GetFromJsonAsync<UserResponse>(API_URL);
            return usersResponse?.Data ?? [];
        }
        catch (Exception ex)
        {
            // In a real application, you would log this exception
            throw new HttpRequestException($"Error fetching users from {API_URL}", ex);
        }
    }
}