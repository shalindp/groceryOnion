using System.ComponentModel.DataAnnotations;
using Application.Enums;

namespace Presentation.Responses.Product;

public record ProductsPriceResponse
{
    [Required] public Guid ProductId { get; set; }
    [Required] public StoreName StoreName { get; init; }
    [Required] public string StoreId { get; init; }
    [Required] public string StoreSku { get; init; }
    [Required] public double Price { get; set; }
}