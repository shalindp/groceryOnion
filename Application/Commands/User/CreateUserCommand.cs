using Application.Interfaces;
using Application.Services;
using Persistence;

namespace Application.Commands.User;

public record CreateUserCommandRequest(string Username, string Password);

public record CreateUserCommandResponse(
    string Username,
    string Token,
    string RefreshToken,
    int TokenExpirationInSeconds);

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

        return new CreateUserCommandResponse(result.Username, result.Token, result.RefreshToken,
            result.TokenExpirationInSeconds);
    }
}