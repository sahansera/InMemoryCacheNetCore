using System.Net.Http.Json;
using InMemoryCachingSample.Models;

namespace InMemoryCachingSample.Infrastructure;

public class UserResponse
{
    public User[] data { get; set; } = Array.Empty<User>();
}

public interface IHttpClient
{
    Task<IEnumerable<User>> Get();
}

public class HttpClient : IHttpClient
{
    private const string API_URL = "https://reqres.in/api/users";
    
    private readonly IHttpClientFactory _clientFactory;

    public HttpClient(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<IEnumerable<User>> Get()
    {
        var client = _clientFactory.CreateClient();

        try
        {
            var usersResponse = await client.GetFromJsonAsync<UserResponse>(API_URL);
            return usersResponse?.data ?? Array.Empty<User>();
        }
        catch (Exception ex)
        {
            // In a real application, you would log this exception
            throw new HttpRequestException($"Error fetching users from {API_URL}", ex);
        }
    }
}