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
        bool freshSession = false);

    Task<HttpResponseWrapper<TResponse>?> PostAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        bool freshSession = false);

    Task<HttpResponseWrapper<TResponse>?> PutAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        bool freshSession = false);

    Task<HttpResponseWrapper<TResponse>?> DeleteAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        bool freshSession = false);

    string? GetCookie(string url, HttpResponseHeaders headers, string cookieName);
}

public class HttpHelper : IHttpHelper
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    // Default HttpClient reused for normal requests
    private readonly HttpClient _defaultClient;

    public HttpHelper()
    {
        _defaultClient = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(30)
        };
    }

    public Task<HttpResponseWrapper<TResponse>?> GetAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        bool freshSession = false)
    {
        return SendAsync<TResponse>(HttpMethod.Get, url, null, headers, freshSession);
    }

    public Task<HttpResponseWrapper<TResponse>?> PostAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        bool freshSession = false)
    {
        return SendAsync<TResponse>(HttpMethod.Post, url, payload, headers, freshSession);
    }

    public Task<HttpResponseWrapper<TResponse>?> PutAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        bool freshSession = false)
    {
        return SendAsync<TResponse>(HttpMethod.Put, url, payload, headers, freshSession);
    }

    public Task<HttpResponseWrapper<TResponse>?> DeleteAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        bool freshSession = false)
    {
        return SendAsync<TResponse>(HttpMethod.Delete, url, null, headers, freshSession);
    }

    private async Task<HttpResponseWrapper<TResponse>?> SendAsync<TResponse>(
        HttpMethod method,
        string url,
        object? payload,
        IDictionary<string, string>? headers,
        bool freshSession)
    {
        HttpClient httpClient;

        if (freshSession)
        {
            // Create a fresh client with a new CookieContainer
            var handler = new HttpClientHandler
            {
                CookieContainer = new CookieContainer(),
                UseCookies = true
            };
            httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }
        else
        {
            // Reuse default HttpClient
            httpClient = _defaultClient;
        }

        using var request = new HttpRequestMessage(method, url);

        // Add headers
        if (headers != null)
        {
            foreach (var (key, value) in headers)
            {
                request.Headers.TryAddWithoutValidation(key, value);
            }
        }

        // Attach payload
        if (payload != null)
        {
            var json = JsonSerializer.Serialize(payload);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

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
                var container = new CookieContainer();
                container.SetCookies(uri, setCookie);

                var cookie = container.GetCookies(uri)[cookieName];
                if (cookie != null)
                    return cookie.Value;
            }
        }

        return null;
    }
}
