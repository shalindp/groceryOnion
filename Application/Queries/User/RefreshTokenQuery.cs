using Application.Interfaces;
using Application.Services;
using Persistence;

namespace Application.Queries.User;

public record RefreshTokenQueryRequest
{
    public string Username { get; init; }
    public string RefreshToken { get; init; }
};

public record RefreshTokenQueryResponse
{
    public string Username { get; init; }
    public string Token { get; init; }
    public string RefreshToken { get; init; }
    public int TokenExpirationInSeconds { get; init; }
}

public class RefreshTokenQuery : ICommand<RefreshTokenQueryResponse, RefreshTokenQueryRequest>
{
    private readonly JwtAuthenticationService _authenticationService;

    public RefreshTokenQuery(JwtAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<RefreshTokenQueryResponse> SendAsync(RefreshTokenQueryRequest request)
    {
        var result =
            await _authenticationService.ValidateRefreshTokenAsync(request.Username, request.RefreshToken);

        return new RefreshTokenQueryResponse
        {
            Username = result.Username,
            Token = result.Token,
            RefreshToken = result.RefreshToken,
            TokenExpirationInSeconds = result.TokenExpirationInSeconds
        };
    }
}