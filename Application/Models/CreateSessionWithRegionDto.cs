using Application.Enums;

namespace Application.Models;

public class CreateSessionWithRegionDto
{
    public StoreName StoreName { get; set; }
    public string Address { get; set; }
    public string SessionId { get; set; }
    public string Aga { get; set; }
}