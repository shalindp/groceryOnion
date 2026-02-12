using Application.Actions.Regions;
using Application.Constants;
using Application.Enums;
using Application.Models;
using Application.Queries.Product;
using Persistence;

namespace Application.Actions.Products;

public interface IWoolworthsProductAction
{
    public Task<QueriesSql.CreateProductsArgs[]> GetStoreProductsAsync(INpgsqlDbContext dbContext);

    public Task<IList<Categoery>> GetAllCategoriesAsync();
    public Task<ProductPriceQueryRequest[]> GetProductPricesAsync(List<WoolworthsStoreSkuAndSessionArg> ags);

    public Task<ProductPriceQueryRequest> GetProductPriceAsync(ProductPriceQueryRequest productPriceQueryRequest, WoolworthsSession session);
}

public record WoolworthsStoreSkuAndSessionArg(ProductPriceQueryRequest ProductPriceQueryRequest, WoolworthsSession Session);

public record WoolworthsPricingResponse(string StoreId, double Price);

public class WoolworthsProductAction : IWoolworthsProductAction
{
    private readonly IHttpHelper _httpHelper;
    private readonly IWoolworthsStoreAction _woolworthsStoreAction;
    private readonly Random _random = new Random();

    public WoolworthsProductAction(IHttpHelper httpHelper,
        IWoolworthsStoreAction woolworthsStoreAction)
    {
        _httpHelper = httpHelper;
        _woolworthsStoreAction = woolworthsStoreAction;
    }

    private record AllProductsResponse(ContextResponse Context, ProductsResponse Products);

    private record ContextResponse(FulfillmentResponse Fulfilment);

    private record FulfillmentResponse(string Address);

    private record ProductsResponse(IList<ItemResponse> Items, int TotalItems);

    private record ItemResponse(
        string Type,
        string Sku,
        string Barcode,
        string Name,
        string Brand,
        ImageResponse Images,
        PriceResponse Price,
        ProductTagResponse ProductTag,
        QuantityResponse Quantity,
        SizeResponse Size
    );

    private record ImageResponse(string Big, string Small);

    private record QuantityResponse(double Max);

    private record PriceResponse(double OriginalPrice, double SalePrice);

    private record ProductTagResponse(MultiBuyResponse? MultiBuy);

    private record MultiBuyResponse(double Quantity, double MultiCupValue);

    private record SizeResponse(string CupMeasure, string VolumesSize, string VolumeSize);

    private async Task<IList<StoreProduct>> GetAllProductsAsync(Dictionary<string, string> headers)
    {
        var url = (string category, int page) =>
            $"https://www.woolworths.co.nz/api/v1/products?dasFilter=Department;;{category};false&target=browse&inStockProductsOnly=false&size=120&page={page}";

        var categories = await GetAllCategoriesAsync();

        var allProducts = new List<StoreProduct>();
        foreach (var category in categories)
        {
            Console.WriteLine($" {StoreName.Woolworths.ToDescription()} | Fetching category: " + category.Name + " ID::" + headers["ASP.NET_SessionId"]);
            for (var page = 1; page <= 1000; page++)
            {
                var response = url(category.Url, page);
                var result =
                    await _httpHelper.GetAsync<AllProductsResponse>(response,
                        headers: headers);

                var products = result!.Body!.Products.Items.Where(c => c.Type == "Product").ToList();

                if (products.Count == 0)
                {
                    break;
                }

                allProducts.AddRange(products.Select(c => new StoreProduct
                {
                    Barcode = c.Barcode,
                    Name = c.Name,
                    Brand = c.Brand,
                    StoreName = StoreName.Woolworths.ToDescription(),
                    ImageUrl = c.Images.Big,
                    MaxQuantity = (int)c.Quantity.Max,
                    UnitAndSize = GetUnitAndSize(c),
                    StoreSku = c.Sku
                }));
            }

            // await Task.Delay(GetRandomTimeoutSeconds());
        }

        return allProducts;
    }

    private string GetUnitAndSize(ItemResponse item)
    {
        if (!string.IsNullOrEmpty(item.Size.VolumeSize))
        {
            return item.Size.VolumeSize;
        }
        else if (!string.IsNullOrEmpty(item.Size.VolumesSize))
        {
            return item.Size.VolumesSize;
        }
        else if (!string.IsNullOrEmpty(item.Size.CupMeasure))
        {
            return item.Size.CupMeasure;
        }
        else
        {
            return "1ea";
        }
    }

    // public async Task<IList<ProductDto>> SearchProductsAsync(string term, int limit, int skip)
    // {
    //     var result = await _dbContext.Queries.SearchProducts(
    //         new QueriesSql.SearchProductsArgs(term, skip, limit));
    //
    //     return result.Select(c => ProductDto.Map(c.CanonicalProduct!.Value!)).ToList();
    // }

    private record ProductPriceResponse(PriceResponse Price);

    public async Task<ProductPriceQueryRequest[]> GetProductPricesAsync(List<WoolworthsStoreSkuAndSessionArg> ags)
    {
        var list = new List<Task<ProductPriceQueryRequest>>();
        foreach (var arg in ags)
        {
            var task = GetProductPriceAsync(arg.ProductPriceQueryRequest, arg.Session);
            list.Add(task);
        }

        return await Task.WhenAll(list);
    }

    public int GetRandomTimeoutSeconds()
    {
        var timeput = _random.Next(200, 220);
        Console.WriteLine($"@> TIMEOUT: {timeput}");
        return timeput;
    }

    public async Task<ProductPriceQueryRequest> GetProductPriceAsync(ProductPriceQueryRequest productPriceQueryRequest, WoolworthsSession session)
    {
        // Console.WriteLine($"@> FETCH SKU:{storeSku} - ADDRESS:{session.AddressId}");
        var url = $"https://www.woolworths.co.nz/api/v1/products/{productPriceQueryRequest.StoreSku}";

        var headers = new Dictionary<string, string>
            {
                ["ASP.NET_SessionId"] = session.SessionId,
                ["aga"] = session.Aga,
            }.Concat(Headers.WoolworthsDefaultHeaders)
            .ToDictionary(k => k.Key, v => v.Value);

        try
        {
            var result = await _httpHelper.GetAsync<ProductPriceResponse>(url, headers: headers, freshSession: true);
            productPriceQueryRequest.Price = result.Body.Price.OriginalPrice;
        }
        catch (Exception e)
        {
            Environment.FailFast("Critical unrecoverable error", e);
        }

        return productPriceQueryRequest;
    }

    public async Task< QueriesSql.CreateProductsArgs[]> GetStoreProductsAsync(INpgsqlDbContext dbContext)
    {
        var products = await GetAllProductsAsync(new Dictionary<string, string>());

        var distinctProducts = products
            .DistinctBy(c => c.Barcode)
            .ToList();

        var skus = distinctProducts.Select(c => c.Barcode).ToArray();

        var existingProductsResult = (await dbContext.Queries.GetStoreProducts(
                new QueriesSql.GetStoreProductsArgs(
                    Skus: skus,
                    StoreName: StoreName.Woolworths.ToDescription()
                ))).Select(c => c.StoreProduct!.Value)
            .ToList();

        var productsToInsert = new List<StoreProduct>();
        foreach (var product in distinctProducts)
        {
            var existingProduct = existingProductsResult.FirstOrDefault(c =>
                c.Barcode == product.Barcode);

            if (existingProduct.StoreProductId != Guid.Empty)
            {
                var nameChanged = existingProduct.Name != product.Name;
                var brandChanged = existingProduct.Brand != product.Brand;
                var imageUrlChanged = existingProduct.ImageUrl != product.ImageUrl;
                var maxQuantityChanged = existingProduct.MaxQuantity != product.MaxQuantity;

                if (nameChanged || brandChanged || imageUrlChanged || maxQuantityChanged)
                {
                    await dbContext.Queries.UpdateStoreProduct(
                        new QueriesSql.UpdateStoreProductArgs()
                        {
                            Barcode = product.Barcode,
                            StoreName = product.StoreName,
                            Name = product.Name,
                            Brand = product.Brand,
                            ImageUrl = product.ImageUrl,
                            MaxQuantity = product.MaxQuantity,
                        });
                }
            }
            else
            {
                productsToInsert.Add(product);
            }
        }

        var barcodeNullThreshold = productsToInsert.Count(c => string.IsNullOrEmpty(c.Barcode)) > 10;
        if (barcodeNullThreshold)
        {
            throw new Exception($"{StoreName.Woolworths.ToDescription()} Too many products with empty barcode, likely an error in fetching products");
        }

        QueriesSql.CreateProductsArgs[] finalProductsToInset =
        [
            ..productsToInsert.Select(c =>
                new QueriesSql.CreateProductsArgs
                {
                    Barcode = c.Barcode,
                    Name = c.Name,
                    Brand = c.Brand,
                    StoreName = StoreName.Woolworths.ToDescription(),
                    ImageUrl = c.ImageUrl,
                    MaxQuantity = c.MaxQuantity,
                    UnitAndSize = c.UnitAndSize,
                    StoreSku = c.StoreSku
                })
        ];
        return finalProductsToInset;
    }

    private record CategoryResponse(IList<SpecialResponse> Specials);

    private record SpecialResponse(int Id, string Label, string Url);


    public async Task<IList<Categoery>> GetAllCategoriesAsync()
    {
        const string url = "https://www.woolworths.co.nz/api/v1/shell";
        var response = await _httpHelper.GetAsync<CategoryResponse>(url, headers: Headers.WoolworthsDefaultHeaders);

        return response!.Body!.Specials
            .Select(c => new Categoery(c.Id, c.Label, c.Url, StoreName.Woolworths))
            .ToList();
    }
}