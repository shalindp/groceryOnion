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
    Task<HttpResponseWrapper<TResponse>> GetAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null,
        bool freshSession = false);

    Task<HttpResponseWrapper<TResponse>> PostAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null,
        bool freshSession = false);

    Task<HttpResponseWrapper<TResponse>> PutAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null,
        bool freshSession = false);

    Task<HttpResponseWrapper<TResponse>> DeleteAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null,
        bool freshSession = false);

    string? GetCookie(string url, HttpResponseHeaders headers, string cookieName);
}

public class HttpHelper : IHttpHelper
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly CookieContainer _cookieContainer;
    private readonly HttpClient _defaultClient;

    public HttpHelper()
    {
        _cookieContainer = new CookieContainer();

        var handler = new HttpClientHandler
        {
            CookieContainer = _cookieContainer,
            UseCookies = true
        };

        _defaultClient = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(30)
        };
    }

    public Task<HttpResponseWrapper<TResponse>> GetAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null,
        bool freshSession = false)
        => SendAsync<TResponse>(HttpMethod.Get, url, null, headers, cookies, freshSession);

    public Task<HttpResponseWrapper<TResponse>> PostAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null,
        bool freshSession = false)
        => SendAsync<TResponse>(HttpMethod.Post, url, payload, headers, cookies, freshSession);

    public Task<HttpResponseWrapper<TResponse>> PutAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null,
        bool freshSession = false)
        => SendAsync<TResponse>(HttpMethod.Put, url, payload, headers, cookies, freshSession);

    public Task<HttpResponseWrapper<TResponse>> DeleteAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null,
        bool freshSession = false)
        => SendAsync<TResponse>(HttpMethod.Delete, url, null, headers, cookies, freshSession);

    private async Task<HttpResponseWrapper<TResponse>> SendAsync<TResponse>(
        HttpMethod method,
        string url,
        object? payload,
        IDictionary<string, string>? headers,
        IDictionary<string, string>? cookies,
        bool freshSession)
    {
        HttpClient client;
        CookieContainer cookieContainer;

        if (freshSession)
        {
            cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                UseCookies = true
            };

            client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }
        else
        {
            client = _defaultClient;
            cookieContainer = _cookieContainer;
        }

        var uri = new Uri(url);

        // Attach cookies manually if provided
        if (cookies != null)
        {
            foreach (var (name, value) in cookies)
            {
                cookieContainer.Add(uri, new Cookie(name, value));
            }
        }

        using var request = new HttpRequestMessage(method, uri);

        if (headers != null)
        {
            foreach (var (key, value) in headers)
            {
                request.Headers.TryAddWithoutValidation(key, value);
            }
        }

        if (payload != null)
        {
            var json = JsonSerializer.Serialize(payload);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        var content = response.Content != null
            ? await response.Content.ReadAsStringAsync()
            : null;

        var body = !string.IsNullOrWhiteSpace(content)
            ? JsonSerializer.Deserialize<TResponse>(content, JsonOptions)
            : default;

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