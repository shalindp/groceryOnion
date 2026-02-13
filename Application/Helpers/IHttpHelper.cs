using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class HttpResponseWrapper<T>
{
    public T? Body { get; set; }
    public HttpResponseHeaders Headers { get; set; } = default!;
}

public interface IHttpHelper
{
    Task<HttpResponseWrapper<TResponse>> GetAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        string? cookies = null,
        IEnumerable<int>? ignoreHttpStatusCodes = null);

    Task<HttpResponseWrapper<TResponse>> PostAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        string? cookies = null,
        IEnumerable<int>? ignoreHttpStatusCodes = null);

    Task<HttpResponseWrapper<TResponse>> PutAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        string? cookies = null,
        IEnumerable<int>? ignoreHttpStatusCodes = null);

    Task<HttpResponseWrapper<TResponse>> DeleteAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        string? cookies = null,
        IEnumerable<int>? ignoreHttpStatusCodes = null);
}

public class HttpHelper : IHttpHelper
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public HttpHelper()
    {
        var handler = new HttpClientHandler
        {
            UseCookies = false, // IMPORTANT: prevent automatic cookie state
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        _httpClient = new HttpClient(handler);

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public Task<HttpResponseWrapper<TResponse>> GetAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        string? cookies = null,
        IEnumerable<int>? ignoreHttpStatusCodes = null)
        => SendAsync<TResponse>(HttpMethod.Get, url, null, headers, cookies, ignoreHttpStatusCodes);

    public Task<HttpResponseWrapper<TResponse>> PostAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        string? cookies = null,
        IEnumerable<int>? ignoreHttpStatusCodes = null)
        => SendAsync<TResponse>(HttpMethod.Post, url, payload, headers, cookies, ignoreHttpStatusCodes);

    public Task<HttpResponseWrapper<TResponse>> PutAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        string? cookies = null,
        IEnumerable<int>? ignoreHttpStatusCodes = null)
        => SendAsync<TResponse>(HttpMethod.Put, url, payload, headers, cookies, ignoreHttpStatusCodes);

    public Task<HttpResponseWrapper<TResponse>> DeleteAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        string? cookies = null,
        IEnumerable<int>? ignoreHttpStatusCodes = null)
        => SendAsync<TResponse>(HttpMethod.Delete, url, null, headers, cookies, ignoreHttpStatusCodes);

    private async Task<HttpResponseWrapper<TResponse>> SendAsync<TResponse>(
        HttpMethod method,
        string url,
        object? payload,
        IDictionary<string, string>? headers,
        string? cookies,
        IEnumerable<int>? ignoreHttpStatusCodes)
    {
        using var request = new HttpRequestMessage(method, url);

        // Payload
        if (payload != null)
        {
            var json = JsonSerializer.Serialize(payload);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        // Headers
        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        // Manual Cookie header (stateless)
        if (!string.IsNullOrWhiteSpace(cookies))
        {
            request.Headers.TryAddWithoutValidation("Cookie", ExtractCookieHeader(cookies));
        }

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode &&
            (ignoreHttpStatusCodes == null ||
             !ignoreHttpStatusCodes.Contains((int)response.StatusCode)))
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"Request failed with status {(int)response.StatusCode}: {errorBody}");
        }

        TResponse? body = default;

        if (response.Content != null)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrWhiteSpace(content))
            {
                body = JsonSerializer.Deserialize<TResponse>(content, _jsonOptions);
            }
        }

        return new HttpResponseWrapper<TResponse>
        {
            Body = body,
            Headers = response.Headers
        };
    }

    private string ExtractCookieHeader(string rawCookieString)
    {
        var parts = rawCookieString.Split(';', StringSplitOptions.RemoveEmptyEntries);

        var validCookies = new List<string>();

        foreach (var part in parts)
        {
            var trimmed = part.Trim();

            // Skip cookie attributes
            if (trimmed.StartsWith("Path=", StringComparison.OrdinalIgnoreCase) ||
                trimmed.StartsWith("Expires=", StringComparison.OrdinalIgnoreCase) ||
                trimmed.StartsWith("Domain=", StringComparison.OrdinalIgnoreCase) ||
                trimmed.StartsWith("Max-Age=", StringComparison.OrdinalIgnoreCase) ||
                trimmed.StartsWith("SameSite=", StringComparison.OrdinalIgnoreCase) ||
                trimmed.Equals("Secure", StringComparison.OrdinalIgnoreCase) ||
                trimmed.Equals("HttpOnly", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (trimmed.Contains("="))
                validCookies.Add(trimmed);
        }

        return string.Join("; ", validCookies);
    }
}
