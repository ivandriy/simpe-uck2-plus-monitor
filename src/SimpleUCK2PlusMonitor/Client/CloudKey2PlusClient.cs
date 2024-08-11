using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SimpleUCK2PlusMonitor.Client.Options;
using SimpleUCK2PlusMonitor.Client.Request;
using SimpleUCK2PlusMonitor.Client.Response;

namespace SimpleUCK2PlusMonitor.Client;

public class CloudKey2PlusClient : ICloudKeyClient
{
    private const string TokenCookieName = "TOKEN";
    
    private readonly HttpClient _client;
    private readonly CloudKeyClientOptions _options;
    private readonly CookieContainer? _cookieContainer = new();

    public bool IsLoggedOn => GetTokenCookie() is not null ? !GetTokenCookie().Expired : false;

    public CloudKey2PlusClient(HttpClient client, IOptions<CloudKeyClientOptions> options)
    {
        _options = options.Value;
        _client = client;
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
    
    public async Task Login()
    {
        var requestBody = new LoginRequest
        {
            UserName = _options.UserName,
            Password = _options.Password,
            Remember = true
        };

        var serializedBody = JsonSerializer.Serialize(requestBody);
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("api/auth/login", UriKind.Relative),
            Content = new StringContent(serializedBody, Encoding.UTF8, "application/json")
        };

        var response = await _client.SendAsync(request);
        
        response.EnsureSuccessStatusCode();
        foreach (var cookieHeader in response.Headers.GetValues("Set-Cookie"))
        {
            _cookieContainer.SetCookies(_client.BaseAddress, cookieHeader);
        }
    }

    public async Task Self()
    {
        var (cookieName, cookieValue) = SetTokenCookie();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("api/users/self", UriKind.Relative),
            Headers = {
            {
                cookieName, cookieValue
            }}
        };
        
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task<SystemInfoResponse> GetSystemInfo()
    {
        var (cookieName, cookieValue) = SetTokenCookie();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("api/system", UriKind.Relative),
            Headers = {
            {
                cookieName, cookieValue
            }}
        };
        
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<SystemInfoResponse>(responseBody);
    }

    private Cookie? GetTokenCookie() 
        =>
        _cookieContainer?.GetCookies(_client.BaseAddress).FirstOrDefault(c =>
            string.Equals(c.Name, TokenCookieName, StringComparison.CurrentCultureIgnoreCase));

    private (string, string) SetTokenCookie() => ("Cookie", $"{TokenCookieName}={GetTokenCookie()?.Value}");
}