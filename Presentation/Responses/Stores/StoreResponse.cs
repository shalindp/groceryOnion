using System.ComponentModel.DataAnnotations;
using Application.Enums;

namespace Presentation.Responses.Stores;

public record StoreResponse
{
    [Required] public string StoreId { get; init; }
    [Required] public string StoreRegionName { get; init; }
    [Required] public StoreName StoreName { get; init; }
}