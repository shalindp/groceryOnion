using Application.Enums;

namespace Application.Models;

public class PricingUrlDto
{
    public StoreName StoreName { get; set; }
    public string Sku { get; set; }

    public string PricingUrl =>
        StoreName switch
        {
            StoreName.Woolworths => $"https://www.woolworths.co.nz/api/v1/products/{Sku}",
            StoreName.PaknSave => $"",
            _ => ""
        };
}