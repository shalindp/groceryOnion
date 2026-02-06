using System.ComponentModel.DataAnnotations;

namespace Presentation.Requests.Authentication;

public class SignInRequest
{
    [Required] public string Username { get; set; }
    [Required] public string Password { get; set; }
}