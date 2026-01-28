namespace Application.Interfaces;

public interface ICommand<T, K>
{
    public Task<T> SendAsync(K requestBody);
}