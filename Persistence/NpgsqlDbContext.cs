using Npgsql;

namespace Persistence;

public class NpgsqlDbContext: INpgsqlDbContext
{
    private readonly string _connectionString;

    public NpgsqlDbContext(string connectionString)
    {
        _connectionString = connectionString;
        Queries = new QueriesSql(connectionString);
    }

    public QueriesSql Queries { get; }

    public async Task<TResult> WithTransactionAsync<TResult>(Func<QueriesSql, Task<TResult>> action)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

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

public interface INpgsqlDbContext
{
    QueriesSql Queries { get; }

    Task<TResult> WithTransactionAsync<TResult>(Func<QueriesSql, Task<TResult>> action);
}