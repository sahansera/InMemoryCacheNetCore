using InMemoryCachingSample.Models;

namespace InMemoryCachingSample.Infrastructure;

public interface IHttpClient
{
    Task<IEnumerable<User>> Get();
}

public class HttpClient(IHttpClientFactory clientFactory) : IHttpClient
{
    private const string UsersEndpoint = "https://jsonplaceholder.typicode.com/users";

    private readonly IHttpClientFactory _clientFactory = clientFactory;

    public async Task<IEnumerable<User>> Get()
    {
        var client = _clientFactory.CreateClient();

        try
        {
            return await client.GetFromJsonAsync<User[]>(UsersEndpoint) ?? [];
        }
        catch (Exception ex)
        {
            // In a real application, you would log this exception
            throw new HttpRequestException($"Error fetching users from {UsersEndpoint}", ex);
        }
    }
}
