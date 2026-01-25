using System.Text;
using System.Text.Json;
using System.Linq;

public interface IHttpHelper
{
    Task<TResponse?> GetAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null);

    Task<TResponse?> PostAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null);

    Task<TResponse?> PutAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null);

    Task<TResponse?> DeleteAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null);
}

public class HttpHelper : IHttpHelper
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public HttpHelper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<TResponse?> GetAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null)
    {
        return SendAsync<TResponse>(HttpMethod.Get, url, null, headers, cookies);
    }

    public Task<TResponse?> PostAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null)
    {
        return SendAsync<TResponse>(HttpMethod.Post, url, payload, headers, cookies);
    }

    public Task<TResponse?> PutAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null)
    {
        return SendAsync<TResponse>(HttpMethod.Put, url, payload, headers, cookies);
    }

    public Task<TResponse?> DeleteAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null)
    {
        return SendAsync<TResponse>(HttpMethod.Delete, url, null, headers, cookies);
    }

    private async Task<TResponse?> SendAsync<TResponse>(
        HttpMethod method,
        string url,
        object? payload,
        IDictionary<string, string>? headers,
        IDictionary<string, string>? cookies)
    {
        using var request = new HttpRequestMessage(method, url);

        // Per-request headers
        if (headers != null)
        {
            foreach (var (key, value) in headers)
            {
                request.Headers.TryAddWithoutValidation(key, value);
            }
        }

        // Attach Cookie header if cookies provided and no Cookie header already present
        if (cookies != null && cookies.Count > 0)
        {
            var hasCookieHeader = headers?.Keys
                .Any(k => string.Equals(k, "Cookie", System.StringComparison.OrdinalIgnoreCase)) ?? false;

            if (!hasCookieHeader)
            {
                var cookieHeader = string.Join("; ", cookies.Select(kv => $"{kv.Key}={kv.Value}"));
                request.Headers.TryAddWithoutValidation("Cookie", cookieHeader);
            }
        }

        // Only attach body when required
        if (payload != null)
        {
            var json = JsonSerializer.Serialize(payload);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        using var response = await _httpClient.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        if (response.Content == null)
            return default;

        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(content))
            return default;

        return JsonSerializer.Deserialize<TResponse>(content, JsonOptions);
    }
}
