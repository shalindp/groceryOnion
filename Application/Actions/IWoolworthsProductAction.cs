using Application.Constants;
using Application.Enums;
using Application.Models;
using Persistence;

namespace Application.Actions;

public interface IWoolworthsProductAction
{
    public Task SyncProductsAsync();
    public Task<IList<Categoery>> GetAllCategoriesAsync();

    public Task<IList<Product>> SearchProductsAsync(string searchTerm, int[] woolworthRegionIds);
}

public class WoolworthsProductAction : IWoolworthsProductAction
{
    private readonly IHttpHelper _httpHelper;
    private readonly INpgsqlDbContext _dbContext;
    private readonly IWoolworthsRegionAction _woolworthsRegionAction;

    public WoolworthsProductAction(IHttpHelper httpHelper, INpgsqlDbContext dbContext,
        IWoolworthsRegionAction woolworthsRegionAction)
    {
        _httpHelper = httpHelper;
        _dbContext = dbContext;
        _woolworthsRegionAction = woolworthsRegionAction;
    }

    private record AllProductsResponse(ContextResponse Context, ProductsResponse Products);

    private record ContextResponse(FulfillmentResponse Fulfilment);

    private record FulfillmentResponse(string Address);

    private record ProductsResponse(IList<ItemResponse> Items);

    private record ItemResponse(
        string Type,
        string Sku,
        string Barcode,
        string Name,
        string Brand,
        ImageResponse Images,
        PriceResponse Price,
        ProductTagResponse ProductTag,
        QuantityResponse Quantity
    );

    private record ImageResponse(string Big, string Small);

    private record QuantityResponse(double Max);

    private record PriceResponse(double OriginalPrice, double SalePrice);

    private record ProductTagResponse(MultiBuyResponse MultiBuy);

    private record MultiBuyResponse(double Quantity, double MultiCupValue);

    private async Task<IList<Product>> GetAllProductsAsync()
    {
        var url = (string category, int page) =>
            $"https://www.woolworths.co.nz/api/v1/products?dasFilter=Department;;{category};false&target=browse&inStockProductsOnly=false&size=120&page={page}";

        var categories = await GetAllCategoriesAsync();

        var allProducts = new List<Product>();
        foreach (var category in categories)
        {
            Console.WriteLine("Fetching category: " + category.Name);
            for (var page = 1; page <= 1000; page++)
            {
                var response = url(category.Url, page);
                var result =
                    await _httpHelper.GetAsync<AllProductsResponse>(response,
                        headers: Headers.WoolworthsDefaultHeaders);

                var products = result!.Body!.Products.Items.Where(c => c.Type == "Product").ToList();

                if (products.Count == 0)
                {
                    break;
                }

                allProducts.AddRange(products.Select(c => new Product
                {
                    Sku = c.Sku,
                    Name = c.Name,
                    Brand = c.Brand,
                    StoreType = (int)StoreType.Woolworths,
                    ImageUrl = c.Images.Big,
                    MaxQuantity = (int)c.Quantity.Max,
                }));

                // Thread.Sleep(200);
            }
        }

        return allProducts;
    }

    public async Task SyncProductsAsync()
    {
        var products = await GetAllProductsAsync();

        var distinctProducts = products
            .DistinctBy(c => c.Sku)
            .ToList();

        var skus = distinctProducts.Select(c => c.Sku).ToArray();

        var existingProductsResult = await _dbContext.Queries.GetWoolworthsProducts(
            new QueriesSql.GetWoolworthsProductsArgs(
                Skus: skus
            ));

        var productsToInsert = new List<Product>();
        foreach (var product in distinctProducts)
        {
            var existingProduct = existingProductsResult.FirstOrDefault(c =>
                c.Sku == product.Sku &&
                c.StoreType == product.StoreType);

            if (existingProduct.Id != Guid.Empty)
            {
                var nameChanged = existingProduct.Name != product.Name;
                var brandChanged = existingProduct.Brand != product.Brand;
                var imageUrlChanged = existingProduct.ImageUrl != product.ImageUrl;
                var maxQuantityChanged = existingProduct.MaxQuantity != product.MaxQuantity;

                if (nameChanged || brandChanged || imageUrlChanged || maxQuantityChanged)
                {
                    await _dbContext.Queries.UpdateProduct(
                        new QueriesSql.UpdateProductArgs
                        {
                            Sku = product.Sku,
                            StoreType = product.StoreType,
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

        await _dbContext.Queries.CreateProducts([
            ..productsToInsert.Select(c =>
                new QueriesSql.CreateProductsArgs
                {
                    Sku = c.Sku,
                    Name = c.Name,
                    Brand = c.Brand,
                    StoreType = c.StoreType,
                    ImageUrl = c.ImageUrl,
                    MaxQuantity = c.MaxQuantity,
                })
        ]);
    }

    private record CategoryResponse(IList<SpecialResponse> Specials);

    private record SpecialResponse(int Id, string Label, string Url);


    public async Task<IList<Categoery>> GetAllCategoriesAsync()
    {
        const string url = "https://www.woolworths.co.nz/api/v1/shell";
        var response = await _httpHelper.GetAsync<CategoryResponse>(url, headers: Headers.WoolworthsDefaultHeaders);

        return response!.Body!.Specials
            .Select(c => new Categoery(c.Id, c.Label, c.Url, StoreType.Woolworths))
            .ToList();
    }

    public async Task<IList<Product>> SearchProductsAsync(string searchTerm, int[] woolworthRegionIds)
    {
        var xx = new List<Task<IList<Product>>>();
        foreach (var woolworthRegionId in woolworthRegionIds)
        {
            var x = SearchProductsForRegionAsync(searchTerm, woolworthRegionId);
            xx.Add(x);
        }

        var t = await Task.WhenAll(xx);
        var xxx = t.SelectMany(c => c).ToList();
        return xxx;
    }

    private async Task<IList<Product>> SearchProductsForRegionAsync(string searchTerm, int woolworthRegionId)
    {
        var products = new List<Product>();
        var session = await _woolworthsRegionAction.CreateSessionWithRegionAsync(woolworthRegionId);

        var url = (string search, int page) =>
            $"https://www.woolworths.co.nz/api/v1/products?target=search&search={search}&inStockProductsOnly=false&size=120&page={page}";

        var cookies = new Dictionary<string, string>
        {
            ["ASP.NET_SessionId"] = session.SessionId,
            ["aga"] = session.Aga
        };

        for (var page = 1; page <= 1000; page++)
        {
            var response = await _httpHelper.GetAsync<AllProductsResponse>(url(searchTerm, page),
                headers: Headers.WoolworthsDefaultHeaders, cookies: cookies);
            var productsResponse = response.Body!.Products.Items;

            if (productsResponse.Count == 0)
            {
                break;
            }

            products.AddRange(productsResponse.Where(c => c.Type == "Product").Select(c => new Product
            {
                Sku = c.Sku,
                Name = c.Name,
                Brand = c.Brand,
                StoreType = (int)StoreType.Woolworths,
                ImageUrl = c.Images.Big,
                MaxQuantity = (int)c.Quantity.Max,
            }));
        }

        var tasks = new List<Task<PriceResponse>>();
        foreach (var itemResponse in products.Take(10))
        {
            var x = Fok(itemResponse.Sku, cookies);
            tasks.Add(x);
        }

        var xx = await Task.WhenAll(tasks);
        Console.WriteLine("DONE");
        return products;
    }

    // private async Task<IList<Product>> Sss(string searchTerm)
    // {
    // var url = (string search)=> $"https://www.woolworths.co.nz/api/v1/products?target=search&search={search}&inStockProductsOnly=false&size=120";

    // }

    private record Hello(PriceResponse price);

    private async Task<PriceResponse> Fok(string sku, Dictionary<string, string> cookies)
    {
        var url = $"https://www.woolworths.co.nz/api/v1/products/{sku}";

        var response =
            await _httpHelper.GetAsync<Hello>(url, headers: Headers.WoolworthsDefaultHeaders, cookies: cookies);

        return response.Body.price;
    }
}