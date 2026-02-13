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
    [Required] public string RegionStoreName { get; set; }
    [Required] public List<ProductMultiBuyResponse> MultiBuys { get; set; } = new List<ProductMultiBuyResponse>();
}

public class ProductMultiBuyResponse
{
    [Required] public double PriceWhenQuantityIsMet { get; init; } = 0;
    [Required] public double QuantityRequired { get; init; } = 0;
}