using System.ComponentModel.DataAnnotations;
using Application.Queries;

namespace Presentation.Responses.Product;

public record GetProductsPricingResponse
{
    [Required] public List<StorePrice> StorePrices { get; init; }
}