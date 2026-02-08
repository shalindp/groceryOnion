namespace Application.Services;

/// <summary>
/// Service to throttle concurrent external API calls
/// </summary>
public interface IWoolworthsThrottleService
{
    /// <summary>
    /// Executes an async function with throttling to limit concurrent calls
    /// </summary>
    /// <typeparam name="T">The return type of the async function</typeparam>
    /// <param name="func">The async function to execute</param>
    /// <returns>The result of the async function</returns>
    Task<T> ExecuteAsync<T>(Func<Task<T>> func);
}

/// <summary>
/// Throttle service implementation using SemaphoreSlim to limit concurrent calls
/// </summary>
public class WoolworthsThrottleService : IWoolworthsThrottleService
{
    private readonly SemaphoreSlim _semaphore;
    private readonly Random _random = new Random();

    public WoolworthsThrottleService(int maxConcurrentCalls)
    {
        _semaphore = new SemaphoreSlim(maxConcurrentCalls, maxConcurrentCalls);
    }

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> func)
    {
        await _semaphore.WaitAsync();
        try
        {
            return await func();
        }
        finally
        {
            await GetRandomTimeoutSeconds();
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Gets a random timeout in seconds between 80 and 120
    /// </summary>
    /// <returns>A random timeout value in seconds</returns>
    public async Task GetRandomTimeoutSeconds()
    {
        var timeput = _random.Next(50, 120);
        await Task.Delay(timeput);
        Console.WriteLine($"@> TIMEOUT: {timeput}");
    }
}
