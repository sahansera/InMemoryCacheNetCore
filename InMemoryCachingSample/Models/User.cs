using System.Text.Json.Serialization;

namespace InMemoryCachingSample.Models;

public class User
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}
