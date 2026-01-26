
namespace Application.Models;

public class Result<T>
{
    public bool IsSuccessful { get; init; }
    public string Message { get; init; } = string.Empty;
    public string[] Errors { get; init; } = [];
    
    public T? Data { get; init; }
    
    public static Result<T?> Success(string message = "") => new Result<T?>
    {
        IsSuccessful = true,
        Message = message
    };
    
    public static Result<T?> Success(T data) => new Result<T?>
    {
        IsSuccessful = true,
        Data = data
    };
    
    public static Result<T?> Failure(string[] errors) => new Result<T?>
    {
        IsSuccessful = false,
        Errors = errors
    };
    
    public static Result<T?> Failure(string error) => new Result<T?>
    {
        IsSuccessful = false,
        Errors = [error]
    };
    
    public static Result<T?> Failure() => new Result<T?>
    {
        IsSuccessful = false,
        Errors = [nameof(T)]
    };
}