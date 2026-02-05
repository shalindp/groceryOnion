using Application.Enums;

namespace Application.Models;

public class ProductDto
{
    public Guid ProductId { get; set; }
    public string Barcode { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public StoreName StoreType { get; set; }
    public string ImageUrl { get; set; }
    public float MaxQuantity { get; set; }
    
    public IList<PricingUrlDto> PricingUrls { get; set; }
}