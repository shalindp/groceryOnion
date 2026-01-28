namespace Application.Interfaces;

public interface IQuery<T, K>
{
    public Task<T> SendAsync(K requestBody);
}