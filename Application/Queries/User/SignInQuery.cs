using Application.Interfaces;
using Application.Services;
using Persistence;

namespace Application.Queries.User;

public record SignInQueryRequest(string Username, string Password);

public record SignInQueryResponse(
    string Username,
    string Token,
    string RefreshToken,
    int TokenExpirationInSeconds);

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

        return new SignInQueryResponse(result.Username, result.Token, result.RefreshToken,
            result.TokenExpirationInSeconds);
    }
}