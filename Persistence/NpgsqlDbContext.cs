using Npgsql;
using Polly;
using Polly.Retry;
using Polly.Timeout;

namespace Persistence;

public class NpgsqlDbContext: INpgsqlDbContext
{
    private readonly string _connectionString;
    private readonly IAsyncPolicy<NpgsqlConnection> _connectionRetryPolicy;

    public NpgsqlDbContext(string connectionString)
    {
        _connectionString = connectionString;
        Queries = new QueriesSql(connectionString);
        _connectionRetryPolicy = CreateConnectionRetryPolicy();
    }

    public QueriesSql Queries { get; }

    /// <summary>
    /// Creates a resilience policy that retries connection attempts with exponential backoff
    /// when the connection pool is exhausted. This is essential under high concurrency loads.
    /// </summary>
    private static IAsyncPolicy<NpgsqlConnection> CreateConnectionRetryPolicy()
    {
        return Policy<NpgsqlConnection>
            .Handle<NpgsqlException>(ex =>
                // Retry on connection pool exhausted or timeout errors
                ex.InnerException is TimeoutException ||
                ex.Message.Contains("connection pool", StringComparison.OrdinalIgnoreCase) ||
                ex.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase) ||
                ex.Message.Contains("no more connections", StringComparison.OrdinalIgnoreCase))
            .Or<TimeoutException>()
            .OrResult(conn => conn == null)
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: retryAttempt =>
                    TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempt) * 100), // Exponential backoff: 200ms, 400ms, 800ms, 1600ms, 3200ms
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"[PostgreSQL] Connection attempt {retryCount} failed. Retrying after {timespan.TotalMilliseconds}ms...");
                });
    }

    public async Task<TResult> WithTransactionAsync<TResult>(Func<QueriesSql, Task<TResult>> action)
    {
        // Execute connection with retry policy
        var connection = await _connectionRetryPolicy.ExecuteAsync(async () =>
        {
            var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            return conn;
        });

        await using (connection)
        {
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var queries = QueriesSql.WithTransaction((NpgsqlTransaction)transaction);
                var result = await action(queries);

                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                //@todo: 
                throw;
            }
        }
    }
}

public interface INpgsqlDbContext
{
    QueriesSql Queries { get; }

    Task<TResult> WithTransactionAsync<TResult>(Func<QueriesSql, Task<TResult>> action);
}