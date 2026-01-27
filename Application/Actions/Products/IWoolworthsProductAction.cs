using Application.Constants;
using Application.Enums;
using Application.Models;
using Persistence;

namespace Application.Actions.Products;

public interface IWoolworthsProductAction
{
    public Task SyncProductsAsync();
    public Task<IList<Categoery>> GetAllCategoriesAsync();
}

public class WoolworthsProductAction : IWoolworthsProductAction
{
    private readonly IHttpHelper _httpHelper;
    private readonly INpgsqlDbContext _dbContext;

    public WoolworthsProductAction(IHttpHelper httpHelper, INpgsqlDbContext dbContext)
    {
        _httpHelper = httpHelper;
        _dbContext = dbContext;
    }

    private record AllProductsResponse(ProductsResponse Products);

    private record ProductsResponse(IList<ItemResponse> Items);

    private record ItemResponse(
        string Type,
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
                    Sku = c.Barcode,
                    Name = c.Name,
                    Brand = c.Brand,
                    StoreType = (int)StoreType.Woolworths,
                    ImageUrl = c.Images.Big,
                    MaxQuantity = (int)c.Quantity.Max,
                }));

                Thread.Sleep(200);
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
}