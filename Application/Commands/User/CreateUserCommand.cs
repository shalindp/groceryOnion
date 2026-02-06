using Application.Interfaces;
using Application.Services;
using Persistence;

namespace Application.Commands.User;

public record CreateUserCommandRequest
{
    public string Username { get; init; }
    public string Password { get; init; }
}

public record CreateUserCommandResponse
{
    public string Username { get; init; }
    public string Token { get; init; }
    public string RefreshToken { get; init; }
    public int TokenExpirationInSeconds { get; init; }
}

public class CreateUserCommand : ICommand<CreateUserCommandResponse, CreateUserCommandRequest>
{
    private readonly INpgsqlDbContext _dbContext;
    private readonly JwtAuthenticationService _authenticationService;

    public CreateUserCommand(JwtAuthenticationService authenticationService, INpgsqlDbContext dbContext)
    {
        _authenticationService = authenticationService;
        _dbContext = dbContext;
    }

    public async Task<CreateUserCommandResponse> SendAsync(CreateUserCommandRequest request)
    {
        var user = (await _dbContext.Queries.createAppUser(new QueriesSql.createAppUserArgs(request.Username,
            request.Password)))?.AppUser;

        if (user == null)
        {
            throw new Exception("Failed to create user.");
        }

        var result =
            await _authenticationService.AuthenticateUser(
                new AuthenticationServiceRequest(user.Value.Username, user.Value.PasswordHash));

        return new CreateUserCommandResponse
        {
            Username = result.Username,
            Token = result.Token,
            RefreshToken = result.RefreshToken,
            TokenExpirationInSeconds = result.TokenExpirationInSeconds
        };
    }
}