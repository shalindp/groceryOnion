namespace Application.Models;

public class Result
{
    public bool IsSuccessful { get; init; }
    public string Message { get; init; } = string.Empty;
    public string[] Errors { get; init; } = [];
    
    public static Result Success(string message = "") => new Result
    {
        IsSuccessful = true,
        Message = message
    };
    
    public static Result Failure(string message, string[] errors) => new Result
    {
        IsSuccessful = false,
        Message = message,
        Errors = errors
    };
    
    public static Result Failure(string message, string error) => new Result
    {
        IsSuccessful = false,
        Message = message,
        Errors = [error]
    };
}