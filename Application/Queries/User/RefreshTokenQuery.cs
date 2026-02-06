using Application.Interfaces;
using Application.Services;
using Persistence;

namespace Application.Queries.User;

public record RefreshTokenQueryRequest
{
    public string Username { get; set; }
    public string RefreshToken { get; set; }
};

public record RefreshTokenQueryResponse(
    string Username,
    string Token,
    string RefreshToken,
    int TokenExpirationInSeconds);

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

        return new RefreshTokenQueryResponse(result.Username, result.Token, result.RefreshToken,
            result.TokenExpirationInSeconds);
    }
}