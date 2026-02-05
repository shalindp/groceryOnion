using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Application.Actions.Regions;
using Application.Enums;
using Persistence;

namespace Application.Actions.Products;

public interface IPaknSaveProductAction
{
    public Task SyncProductsAsync();
}

public class PaknSaveProductAction : IPaknSaveProductAction
{
    private readonly IHttpHelper _httpHelper;
    private readonly INpgsqlDbContext _dbContext;
    private readonly IWoolworthsRegionAction _woolworthsRegionAction;

    public PaknSaveProductAction(IHttpHelper httpHelper, INpgsqlDbContext dbContext,
        IWoolworthsRegionAction woolworthsRegionAction)
    {
        _httpHelper = httpHelper;
        _dbContext = dbContext;
        _woolworthsRegionAction = woolworthsRegionAction;
    }

    public async Task SyncProductsAsync()
    {
        var products = await GetAllProductsAsync();

        var distinctProducts = products
            .DistinctBy(c => c.Barcode)
            .ToList();

        var skus = distinctProducts.Select(c => c.Barcode).ToArray();

        var existingProductsResult = (await _dbContext.Queries.GetStoreProducts(
                new QueriesSql.GetStoreProductsArgs(
                    Skus: skus,
                    StoreName: StoreName.PaknSave.ToDescription()
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
                    await _dbContext.Queries.UpdateStoreProduct(
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
            throw new Exception($"{StoreName.PaknSave.ToDescription()} Too many products with empty barcode, likely an error in fetching products");
        }

        await _dbContext.Queries.CreateProducts([
            ..productsToInsert.Select(c =>
                new QueriesSql.CreateProductsArgs
                {
                    Barcode = !string.IsNullOrEmpty(c.Barcode)? c.Barcode: "",
                    Name = c.Name,
                    Brand = c.Brand,
                    StoreName = StoreName.PaknSave.ToDescription(),
                    ImageUrl = c.ImageUrl,
                    MaxQuantity = c.MaxQuantity,
                    UnitAndSize = c.UnitAndSize,
                })
        ]);
    }

    private record ProductsResponse(ProductResponse[] Products, int TotalPages);

    private record ProductResponse(string ProductId, string Name, string Brand, string DisplayName)
    {
        public string Sku { get; set; }
    };

    private record ProductDetailsResponse(string ProductId, string Sku);

    private async Task<ProductResponse[]> GetProductsDetailsAsync(ProductResponse[] products,
        Dictionary<string, string> headers)
    {
        var tasks = new List<Task<HttpResponseWrapper<ProductDetailsResponse>?>>();
        foreach (var product in products)
        {
            var url =
                $"https://api-prod.paknsave.co.nz/v1/edge/store/3404c253-577f-45ca-b301-c98312e46efb/product/{product.ProductId}";

            var task = _httpHelper.GetAsync<ProductDetailsResponse>(url, headers: headers);
            tasks.Add(task);
        }

        var resposne = await Task.WhenAll(tasks);

        foreach (var productResponse in products)
        {
            foreach (var httpResponseWrapper in resposne)
            {
                if (httpResponseWrapper!.Body!.ProductId == productResponse.ProductId)
                {
                    productResponse.Sku = httpResponseWrapper.Body.Sku;
                }
            }
        }

        return products;
    }

    private async Task<IList<StoreProduct>> GetAllProductsAsync()
    {
        var tokenResult = await CreateAccessTokenAsync();
        var headers = GetAuthentication(tokenResult);
        var categories = await GetAllCategoriesAsync(headers);

        const string url =
            "https://api-prod.paknsave.co.nz/v1/edge/search/paginated/products";

        var products = new List<ProductResponse>();
        foreach (var category in categories)
        {
            Console.WriteLine($" {StoreName.PaknSave.ToDescription()} | Fetching category: " + category.Name);

            var initialPayload = GetProductsQueryPayload(category.Name, 0);
            var initialProductResponse = await _httpHelper.PostAsync<ProductsResponse>(url, initialPayload, headers);
            var initialReturnedProducts = initialProductResponse.Body!.Products;
            var maxPages = initialProductResponse.Body.TotalPages;

            var initialWithSku = await GetProductsDetailsAsync(initialReturnedProducts, headers);
            products.AddRange(initialWithSku);

            for (var page = 1; page <= maxPages; page++)
            {
                var payload = GetProductsQueryPayload(category.Name, page);
                var productResponse = await _httpHelper.PostAsync<ProductsResponse>(url, payload, headers);

                var returnedProducts = productResponse.Body!.Products;
                var withSku = await GetProductsDetailsAsync(returnedProducts, headers);
                products.AddRange(withSku);
            }
        }

        return products.Select(c => new StoreProduct
        {
            Barcode = c.Sku,
            Name = c.Name,
            Brand = c.Brand,
            StoreName = StoreName.PaknSave.ToDescription(),
            ImageUrl = $"https://a.fsimg.co.nz/product/retail/fan/image/400x400/{c.ProductId.Split('-')[0]}.png?w=256",
            MaxQuantity = 999,
            UnitAndSize = c.DisplayName,
        }).ToList();
    }

    private record CategoryResponse(string Name);

    private async Task<CategoryResponse[]> GetAllCategoriesAsync(Dictionary<string, string> headers)
    {
        const string url =
            "https://api-prod.paknsave.co.nz/v1/edge/store/3404c253-577f-45ca-b301-c98312e46efb/categories";

        var response = await _httpHelper.GetAsync<CategoryResponse[]>(url, headers);
        return response.Body!;
    }

    private record CreateTokenRequest(string fingerprintGuest, string fingerprintUser);

    private record CreateTokenResponse(string access_token);

    private async Task<CreateTokenResponse> CreateAccessTokenAsync()
    {
        const string url = "https://www.paknsave.co.nz/api/user/get-current-user";
        var body = new Dictionary<string, string>()
        {
            ["fingerprintGuest"] =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/144.0.0.0 Safari/537.36",
            ["fingerprintUser"] = GenerateRandomHex32()
        };

        var response = await _httpHelper.PostAsync<CreateTokenResponse>(url, payload: body);
        return response.Body!;
    }

    public static string GenerateRandomHex32()
    {
        var bytes = new byte[16];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private Dictionary<string, string> GetAuthentication(CreateTokenResponse tokenResponse)
    {
        return new Dictionary<string, string>()
        {
            ["authorization"] = $"Bearer {tokenResponse.access_token}"
        };
    }

    private (double size, string unit) ParseSize(string name, string displayName)
    {
        try
        {
            var regex = new Regex(@"^\s*([\d.,]+)\s*([a-zA-Z]+)\s*$", RegexOptions.IgnoreCase);
            var match = regex.Match(displayName);

            if (!match.Success)
                Console.WriteLine($"ERROR: Could not parse size for (match unsucessful) {name}, {displayName}");
            // throw new Exception($"Could not parse size for (match unsucessful) {displayName}");

            var numberPart = match.Groups[1].Value.Replace(",", "."); // handle comma decimals
            var unitPart = match.Groups[2].Value.ToLower(); // normalize unit to lowercase

            if (!double.TryParse(numberPart, out double value))
                Console.WriteLine($"ERROR: Could not parse size for {name}, {displayName}");
            // throw new Exception($"Could not parse size for {displayName}");

            return (size: value, unit: unitPart);
        }
        catch
        {
            return (0, "");
        }
    }

    private object GetProductsQueryPayload(string category, int page)
    {
        return new
        {
            algoliaQuery = new
            {
                attributesToHighlight = Array.Empty<string>(),
                attributesToRetrieve = new[]
                {
                    "productID",
                    "Type",
                    "sponsored",
                    "category0NI",
                    "category1NI",
                    "category2NI"
                },
                facets = new[]
                {
                    "brand",
                    "category1NI",
                    "onPromotion",
                    "productFacets",
                    "tobacco"
                },
                filters = $"stores:3404c253-577f-45ca-b301-c98312e46efb AND category0NI:\"{category}\"",
                highlightPostTag = "__/ais-highlight__",
                highlightPreTag = "__ais-highlight__",
                hitsPerPage = 50,
                maxValuesPerFacet = 100,
                page = 0,
                analyticsTags = new[]
                {
                    "fs#WEB:mobile"
                }
            },
            algoliaFacetQueries = Array.Empty<object>(),
            storeId = "3404c253-577f-45ca-b301-c98312e46efb",
            hitsPerPage = 50,
            page = page,
            sortOrder = "NI_POPULARITY_ASC",
            tobaccoQuery = false,
            precisionMedia = new
            {
                adDomain = "CATEGORY_PAGE",
                adPositions = new[] { 4, 8, 12 },
                publishImpressionEvent = false,
                disableAds = true
            }
        };
    }
}