using System.Text.Json.Serialization;

namespace SimpleUCK2PlusMonitor.Client.Request;

internal class LoginRequest
{
    [JsonPropertyName("username")]
    public string UserName { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("remember")]
    public bool Remember { get; set; }
}