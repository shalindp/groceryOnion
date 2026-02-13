using System.ComponentModel.DataAnnotations;

namespace Presentation.Responses;

public class CreateStoreSessionsResponse
{
    [Required] public WoolworthSessionResponse[] WoolworthsSessions { get; set; }
    [Required] public string AccessToken {get; set; }
}