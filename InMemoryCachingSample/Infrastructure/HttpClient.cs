using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using InMemoryCachingSample.Models;

namespace InMemoryCachingSample.Infrastructure
{
    public interface IHttpClient
    {
        Task<IEnumerable<User>> Get();
    }
    public class HttpClient : IHttpClient
    {
        private readonly IHttpClientFactory _clientFactory;

        public HttpClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<User>> Get()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://reqres.in/api/users");
            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                var users = await JsonSerializer.DeserializeAsync
                    <IEnumerable<User>>(responseStream);
                return users;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}