using System.ComponentModel.DataAnnotations;

namespace Presentation.Responses.Authentication;

public class SignInResponse
{
    [Required] public string Username { get; set; }
    [Required] public string Token { get; set; }
    [Required] public string RefreshToken {get; set; }
    [Required] public int TokenExpirationInSeconds { get; set; } = 0;
}