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
        bool freshSession = false,
        IEnumerable<int>? ignoreHttpStatusCodes = null);

    Task<HttpResponseWrapper<TResponse>> PostAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null,
        bool freshSession = false,
        IEnumerable<int>? ignoreHttpStatusCodes = null);

    Task<HttpResponseWrapper<TResponse>> PutAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null,
        bool freshSession = false,
        IEnumerable<int>? ignoreHttpStatusCodes = null);

    Task<HttpResponseWrapper<TResponse>> DeleteAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null,
        bool freshSession = false,
        IEnumerable<int>? ignoreHttpStatusCodes = null);

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
        bool freshSession = false,
        IEnumerable<int>? ignoreHttpStatusCodes = null)
        => SendAsync<TResponse>(HttpMethod.Get, url, null, headers, cookies, freshSession, ignoreHttpStatusCodes);

    public Task<HttpResponseWrapper<TResponse>> PostAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null,
        bool freshSession = false,
        IEnumerable<int>? ignoreHttpStatusCodes = null)
        => SendAsync<TResponse>(HttpMethod.Post, url, payload, headers, cookies, freshSession, ignoreHttpStatusCodes);

    public Task<HttpResponseWrapper<TResponse>> PutAsync<TResponse>(
        string url,
        object? payload = null,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null,
        bool freshSession = false,
        IEnumerable<int>? ignoreHttpStatusCodes = null)
        => SendAsync<TResponse>(HttpMethod.Put, url, payload, headers, cookies, freshSession, ignoreHttpStatusCodes);

    public Task<HttpResponseWrapper<TResponse>> DeleteAsync<TResponse>(
        string url,
        IDictionary<string, string>? headers = null,
        IDictionary<string, string>? cookies = null,
        bool freshSession = false,
        IEnumerable<int>? ignoreHttpStatusCodes = null)
        => SendAsync<TResponse>(HttpMethod.Delete, url, null, headers, cookies, freshSession, ignoreHttpStatusCodes);

    private async Task<HttpResponseWrapper<TResponse>> SendAsync<TResponse>(
        HttpMethod method,
        string url,
        object? payload,
        IDictionary<string, string>? headers,
        IDictionary<string, string>? cookies,
        bool freshSession,
        IEnumerable<int>? ignoreHttpStatusCodes = null)
    {
        const int maxRetries = 5;
        int retryCount = 0;
        Exception? lastException = null;
        var ignoreStatusCodeSet = ignoreHttpStatusCodes?.ToHashSet() ?? new HashSet<int>();

        while (retryCount < maxRetries)
        {
            try
            {
                return await ExecuteRequestAsync<TResponse>(method, url, payload, headers, cookies, freshSession, ignoreStatusCodeSet);
            }
            catch (HttpRequestException ex)
            {
                retryCount++;
                lastException = ex;

                // Check if this is an ignored status code
                if (ignoreStatusCodeSet.Count > 0 && int.TryParse(ex.StatusCode?.GetHashCode().ToString(), out var statusCode))
                {
                    if (ignoreStatusCodeSet.Contains((int)ex.StatusCode))
                    {
                        // Don't retry, just throw the exception
                        throw;
                    }
                }

                if (retryCount < maxRetries)
                {
                    // Add exponential backoff: 1s, 2s, 4s
                    var delayMs = (int)Math.Pow(2, retryCount - 1) * 1000;
                    Console.WriteLine($"[HttpHelper] URL: {url} threw {ex.GetType().Name}: {ex.Message} - Retrying {retryCount}/5 after {delayMs}ms delay");
                    await Task.Delay(delayMs);
                }
                else
                {
                    var prevColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine($"[HttpHelper] URL: {url} threw {ex.GetType().Name}: {ex.Message} - All {maxRetries} retries exhausted");

                    Console.ForegroundColor = prevColor;
                }
            }
            catch (Exception ex)
            {
                retryCount++;
                lastException = ex;

                if (retryCount < maxRetries)
                {
                    // Add exponential backoff: 1s, 2s, 4s
                    var delayMs = (int)Math.Pow(2, retryCount - 1) * 1000;
                    Console.WriteLine($"[HttpHelper] URL: {url} threw {ex.GetType().Name}: {ex.Message} - Retrying {retryCount}/5 after {delayMs}ms delay");
                    await Task.Delay(delayMs);
                }
                else
                {
                    var prevColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine($"[HttpHelper] URL: {url} threw {ex.GetType().Name}: {ex.Message} - All {maxRetries} retries exhausted");

                    Console.ForegroundColor = prevColor;
                }
            }
        }

        // If all retries failed, throw the last exception
        throw lastException!;
    }

    private async Task<HttpResponseWrapper<TResponse>> ExecuteRequestAsync<TResponse>(
        HttpMethod method,
        string url,
        object? payload,
        IDictionary<string, string>? headers,
        IDictionary<string, string>? cookies,
        bool freshSession,
        HashSet<int> ignoreHttpStatusCodes = null)
    {
        ignoreHttpStatusCodes ??= new HashSet<int>();

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
        
        // Check if status code should be ignored
        if (!response.IsSuccessStatusCode && ignoreHttpStatusCodes.Contains((int)response.StatusCode))
        {
            // Return response even if not successful
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

        response.EnsureSuccessStatusCode();

        var responseContent = response.Content != null
            ? await response.Content.ReadAsStringAsync()
            : null;

        var responseBody = !string.IsNullOrWhiteSpace(responseContent)
            ? JsonSerializer.Deserialize<TResponse>(responseContent, JsonOptions)
            : default;

        return new HttpResponseWrapper<TResponse>
        {
            Body = responseBody,
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