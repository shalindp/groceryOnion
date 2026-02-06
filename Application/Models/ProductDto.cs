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

public class PricingUrlDto
{
    public StoreName StoreName { get; set; }
    public string Sku { get; set; }

    public string PricingUrl =>
        StoreName switch
        {
            StoreName.Woolworths => $"https://www.woolworths.co.nz/api/v1/products/{Sku}",
            StoreName.PaknSave => $"https://api-prod.paknsave.co.nz/v1/edge/store/[[storeId]]/product/{Sku}",
            _ => ""
        };
}
