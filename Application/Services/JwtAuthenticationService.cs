using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace Application.Services;

public class JwtAuthenticationService
{
    private readonly INpgsqlDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public JwtAuthenticationService(INpgsqlDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task<AuthenticationServiceResponse> AuthenticateUser(AuthenticationServiceRequest request)
    {
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            throw new ArgumentException("Username and password must be provided.");
        }

        var appUser = (await _dbContext.Queries.getAppUser(new QueriesSql.getAppUserArgs(request.Username)))?.AppUser;
        if (appUser == null || appUser.Value.PasswordHash != request.Password)
        {
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        return await GenerateJwtTokenAsync(appUser.Value);
    }

    public async Task<AuthenticationServiceResponse> ValidateRefreshTokenAsync(string userName, string refreshToken)
    {
        var appUser = (await _dbContext.Queries.getAppUser(new QueriesSql.getAppUserArgs(userName)))?.AppUser;

        var existingRefreshToken =
            (await _dbContext.Queries.getRefreshToken(new QueriesSql.getRefreshTokenArgs(appUser!.Value.AppUserId)))
            ?.RefreshToken;

        if (existingRefreshToken == null)
        {
            throw new Exception("Refresh token not found.");
        }
        
        await _dbContext.Queries.deleteRefreshToken(
            new QueriesSql.deleteRefreshTokenArgs(appUser.Value.AppUserId, existingRefreshToken.Value.Token));
        
        
        return await GenerateJwtTokenAsync(appUser.Value);
    }

    private async Task<AuthenticationServiceResponse> GenerateJwtTokenAsync(AppUser appUser)
    {
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var secretKey = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!);
        var tokenValidityInMinutes = int.Parse(_configuration["Jwt:TokenValidityInMinutes"]!);
        var tokenExpiryTimestamp = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes);

        var token = new JwtSecurityToken(issuer, audience, [
                new Claim(ClaimTypes.NameIdentifier, appUser.AppUserId.ToString()),
                new Claim(ClaimTypes.Name, appUser.Username),
            ], expires: tokenExpiryTimestamp,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha512Signature));

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        var refreshToken = await GenerateRefreshTokenAsync(appUser.AppUserId);

        return new AuthenticationServiceResponse(
            appUser.Username,
            accessToken,
            refreshToken,
            (int)tokenExpiryTimestamp.Subtract(DateTime.UtcNow).TotalSeconds);
    }

    private async Task<string> GenerateRefreshTokenAsync(Guid userId)
    {
        var tokenValidityInMinutes = int.Parse(_configuration["Jwt:RefreshTokenValidityInMinutes"]!);
        var token = (await _dbContext.Queries.createRefreshToken(new QueriesSql.createRefreshTokenArgs
        {
            Token = Guid.NewGuid().ToString(),
            ExpiresUtc = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
            AppUserId = userId,
        }))?.RefreshToken;

        return token?.Token ?? throw new Exception("Failed to create refresh token.");
    }
}

public record AuthenticationServiceRequest(string Username, string Password);

public record AuthenticationServiceResponse(
    string Username,
    string Token,
    string RefreshToken,
    int TokenExpirationInSeconds);