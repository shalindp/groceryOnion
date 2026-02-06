using System.ComponentModel.DataAnnotations;

namespace Presentation.Requests.Authentication;

public class RefreshRequest
{
    [Required] public string Username { get; set; }
    [Required] public string RefreshToken { get; set; }
}