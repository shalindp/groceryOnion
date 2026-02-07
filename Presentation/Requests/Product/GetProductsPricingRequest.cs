using System.ComponentModel.DataAnnotations;
using Application.Enums;

namespace Presentation.Requests.Product;

public record GetProductsPricingRequest
{
    [Required] public int[] WoolworthStoreIds { get; init; }

    [Required] public string[] PaknSaveStoreIds { get; init; }

    [Required] public StoreSkuRequest[] StoreSkus { get; init; }
}

public class StoreSkuRequest
{
    public Guid ProductId { get; set; }
    public StoreName StoreName { get; init; }
    public string StoreSku { get; init; }
}