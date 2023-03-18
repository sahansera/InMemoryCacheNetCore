using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using InMemoryCachingSample.Models;

namespace InMemoryCachingSample.Infrastructure
{
    public class UserResponse
    {
        public User[] data { get; set; }
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
            var request = new HttpRequestMessage(HttpMethod.Get, API_URL);
            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();

                var usersResponse = await JsonSerializer.DeserializeAsync
                    <UserResponse>(responseStream);
                var users = usersResponse.data;

                return users;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}