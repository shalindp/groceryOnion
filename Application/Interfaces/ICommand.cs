namespace Application.Interfaces;

public interface ICommand<T, K>
{
    public Task<T> SendAsync(K request);
}

public interface ICommand<T>
{
    public Task<T> SendAsync();
}