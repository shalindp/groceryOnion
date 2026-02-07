using Application.Enums;

namespace Application.Models;

public class ProductDto
{
    public Guid ProductId { get; set; }
    public string Barcode { get; set; }
    public List<StoreSkuDto> StoreSkus { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public string ImageUrl { get; set; }
    public float MaxQuantity { get; set; }
}

public class StoreSkuDto
{
    public Guid ProductId { get; set; }
    public StoreName StoreName { get; init; }
    public string StoreSku { get; init; }
}