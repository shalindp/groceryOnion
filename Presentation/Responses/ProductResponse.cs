using System.ComponentModel.DataAnnotations;
using Application.Enums;

namespace Presentation.Responses;

public record ProductResponse
{
    [Required] public Guid ProductId { get; set; }
    [Required] public string Barcode { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string Brand { get; set; }
    [Required] public StoreName StoreType { get; set; }
    [Required] public string ImageUrl { get; set; }
    [Required] public float MaxQuantity { get; set; }
    
    [Required] public IList<PricingUrlResponse> PricingUrls { get; set; }
}

public record PricingUrlResponse
{
    public StoreName StoreName { get; set; }
    public string Sku { get; set; }
    public string PricingUrl { get; set; }
}