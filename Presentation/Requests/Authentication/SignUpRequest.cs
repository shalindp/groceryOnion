using System.ComponentModel.DataAnnotations;

namespace Presentation.Requests.Authentication;

public class SignUpRequest
{
    [Required] public string Username { get; set; }
    [Required] public string Password { get; set; }
}