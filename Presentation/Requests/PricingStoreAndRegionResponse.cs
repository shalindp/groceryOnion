using System.ComponentModel.DataAnnotations;
using Application.Enums;

namespace Presentation.Requests;

public record CreateSessionWithRegionId(
    [Required] StoreName StoreName,
    [Required] int RegionId
);