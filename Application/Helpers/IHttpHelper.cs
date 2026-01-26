using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
public class HttpResponseWrapper<T>
{
    public T? Body { get; set; }
    public HttpResponseHeaders Headers { get; set; } = default!;
    public IEnumerable<string>? SetCookies => Headers.TryGetValues("Set-Cookie", out var values) ? values : null;
}

public interface IHttpHelper
{
    Task<HttpResponseWrapper<TResponse>?> GetAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null);

    Task<HttpResponseWrapper<TResponse>?> PostAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null);

    Task<HttpResponseWrapper<TResponse>?> PutAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null);

    Task<HttpResponseWrapper<TResponse>?> DeleteAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null);
    
    public string? GetCookie(string url, HttpResponseHeaders headers, string cookieName);
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

    public Task<HttpResponseWrapper<TResponse>?> GetAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null)
    {
        return SendAsync<TResponse>(HttpMethod.Get, url, null, headers, cookies);
    }

    public Task<HttpResponseWrapper<TResponse>?> PostAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null)
    {
        return SendAsync<TResponse>(HttpMethod.Post, url, payload, headers, cookies);
    }

    public Task<HttpResponseWrapper<TResponse>?> PutAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null)
    {
        return SendAsync<TResponse>(HttpMethod.Put, url, payload, headers, cookies);
    }

    public Task<HttpResponseWrapper<TResponse>?> DeleteAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null)
    {
        return SendAsync<TResponse>(HttpMethod.Delete, url, null, headers, cookies);
    }

    private async Task<HttpResponseWrapper<TResponse>?> SendAsync<TResponse>(
        HttpMethod method,
        string url,
        object? payload,
        IDictionary<string, string>? headers,
        IDictionary<string, string>? cookies)
    {
        using var request = new HttpRequestMessage(method, url);

        // Add headers
        if (headers != null)
        {
            foreach (var (key, value) in headers)
            {
                request.Headers.TryAddWithoutValidation(key, value);
            }
        }

        // Attach cookies
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

        // Attach payload
        if (payload != null)
        {
            var json = JsonSerializer.Serialize(payload);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        string? content = null;
        if (response.Content != null)
        {
            content = await response.Content.ReadAsStringAsync();
        }

        TResponse? body = default;
        if (!string.IsNullOrWhiteSpace(content))
        {
            body = JsonSerializer.Deserialize<TResponse>(content, JsonOptions);
        }

        return new HttpResponseWrapper<TResponse>
        {
            Body = body,
            Headers = response.Headers
        };
    }
    
    public string? GetCookie(string url, HttpResponseHeaders headers, string cookieName)
    {
        if (headers.TryGetValues("Set-Cookie", out var setCookieHeaders))
        {
            foreach (var setCookie in setCookieHeaders)
            {
                var uri = new Uri(url);
                // Use CookieContainer to parse the cookie safely
                var container = new CookieContainer();
                container.SetCookies(uri, setCookie);

                // Now get the cookie by name
                var cookie = container.GetCookies(uri)[cookieName];
                if (cookie == null) continue;

                var value = cookie.Value;
                return value;
            }
        }

        return null;
    }
}