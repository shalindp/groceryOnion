using System.ComponentModel.DataAnnotations;
using Application.Enums;

namespace Presentation.Responses;

public class CreateSessionWithRegionResponse
{
    [Required] public StoreName StoreName { get; set; }
    [Required] public string Address { get; set; }
    [Required] public string SessionId { get; set; }
    [Required] public string Aga { get; set; }
}