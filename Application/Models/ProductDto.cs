using Application.Enums;
using Persistence;

namespace Application.Models;

public class ProductDto
{
    public string Sku { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public short StoreType { get; set; }
    public string ImageUrl { get; set; }
    public decimal MaxQuantity { get; set; }
    public IList<PricingUrlDto> PricingUrls { get; set; } = new List<PricingUrlDto>();

    public static ProductDto Map(CanonicalProduct source)
    {
        return new ProductDto
        {
            // Sku = source.Sku,
            // Name = source.,
            // Brand = source.Brand,
            // StoreType = source.StoreType,
            // ImageUrl = source.ImageUrl,
            // MaxQuantity = source.MaxQuantity,
            // PricingUrls =
            // [
            //     new PricingUrlDto
            //     {
            //         StoreName = StoreName.Woolworths,
            //         Sku = source.Sku,
            //     }
            // ]
        };
    }
}