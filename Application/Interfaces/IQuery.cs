namespace Application.Interfaces;

public interface IQuery<T, K>
{
    public Task<T> SendAsync(K request);
}

public interface IQuery<T>
{
    public Task<T> SendAsync();
}