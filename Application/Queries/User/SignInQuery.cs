using Application.Interfaces;
using Application.Services;
using Persistence;

namespace Application.Queries.User;

public record SignInQueryRequest
{
    public string Username { get; init; }
    public string Password { get; init; }
}

public record SignInQueryResponse
{
    public string Username { get; init; }
    public string Token { get; init; }
    public string RefreshToken { get; init; }
    public int TokenExpirationInSeconds { get; init; }
}

public class SignInQuery : ICommand<SignInQueryResponse, SignInQueryRequest>
{
    private readonly JwtAuthenticationService _authenticationService;

    public SignInQuery(JwtAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<SignInQueryResponse> SendAsync(SignInQueryRequest request)
    {
        var result =
            await _authenticationService.AuthenticateUser(
                new AuthenticationServiceRequest(request.Username, request.Password));

        return new SignInQueryResponse
        {
            Username = result.Username,
            Token = result.Token,
            RefreshToken = result.RefreshToken,
            TokenExpirationInSeconds = result.TokenExpirationInSeconds
        };
    }
}