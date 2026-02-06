using System.ComponentModel.DataAnnotations;
using Application.Queries;

namespace Presentation.Requests.Product;

public record GetProductsPricingRequest
{
    [Required] public int[] WoolworthStoreIds { get; init; }

    [Required] public string[] PaknSaveStoreIds { get; init; }

    [Required] public ProductIdAndStoreSku[] ProductIdAndStoreSkus { get; init; }
}