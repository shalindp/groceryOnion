using Application.Enums.Products;
using Application.Models.Products;

namespace Application.Products.Actions;

public interface IWoolworthsProductAction
{
    public Task<IList<Product>> GetAllProductsAsync(string region, string sessionId, string aga);
}

public class WoolworthsProductAction : IWoolworthsProductAction
{
    private readonly IHttpHelper _httpHelper;

    private const string BaseUrl = "https://www.woolworths.co.nz";

    private readonly IList<CategoryAndMaxPage> _categoriesAndMaxPages = new List<CategoryAndMaxPage>()
    {
        new CategoryAndMaxPage("fruit-veg", 4),
        new CategoryAndMaxPage("meat-poultry", 4),
        new CategoryAndMaxPage("fish-seafood", 2),
        new CategoryAndMaxPage("fridge-deli", 16),
        new CategoryAndMaxPage("bakery", 6),
        new CategoryAndMaxPage("frozen", 8),
        new CategoryAndMaxPage("pantry", 46),
        new CategoryAndMaxPage("beer-wine", 12),
        new CategoryAndMaxPage("drinks", 10),
        new CategoryAndMaxPage("health-body", 28),
        new CategoryAndMaxPage("household", 18),
        new CategoryAndMaxPage("baby-child", 6),
        new CategoryAndMaxPage("pets", 6),
    };


    private record CategoryAndMaxPage(string Category, int MaxPage);

    public WoolworthsProductAction(IHttpHelper httpHelper)
    {
        _httpHelper = httpHelper;
    }

    private record WoolworthsProductPrice(
        double OriginalPrice,
        bool CanShowOriginalPrice,
        double SalePrice,
        bool CanShowSalePrice);

    private record WoolworthsProductImage(string Big, string Small);

    private record WoolworthsItem(string Name, WoolworthsProductPrice? Price, WoolworthsProductImage? Image);

    private record WoolworthsProduct(IList<WoolworthsItem> Items);

    private record WoolworthsSearchResult(WoolworthsProduct Products, WoolworthsContext Context);

    private record WoolworthsFulfillment(string Address, int AreaId);

    private record WoolworthsContext(WoolworthsFulfillment Fulfilment);

    public async Task<IList<Product>> GetAllProductsAsync(string region, string sessionId, string aga)
    {
        var url = (string category, int page) =>
            $"{BaseUrl}/api/v1/products?dasFilter=Department;;{category};false&target=browse&inStockProductsOnly=true&size=120&page={page}";

        var headers = new Dictionary<string, string>
        {
            ["User-Agent"] =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3",
            ["x-requested-with"] = "OnlineShopping.WebApp"
        };

        var cookies = new Dictionary<string, string>
        {
            { "aga", aga },
            { "ASP.NET_SessionId", sessionId }
        };

        // Thread-safe limit: only 3 API calls may run at the same time
        var semaphore = new SemaphoreSlim(3);

        // Shared product collection (needs locking when writing)
        var products = new List<Product>();

        // Keep track of all running tasks so we can await them later
        var tasks = new List<Task>();

        foreach (var categoryAndMaxPage in _categoriesAndMaxPages)
        {
            for (var page = 1; page <= categoryAndMaxPage.MaxPage; page++)
            {
                Console.WriteLine($"products total: {products.Count}");
                // Wait until one of the 3 "slots" is available
                await semaphore.WaitAsync();

                // Capture loop variables to avoid closure issues
                var category = categoryAndMaxPage.Category;
                var currentPage = page;

                // Fire off the work without blocking the loop
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        // Build the final URL for this category + page
                        var finalUrl = url(category, currentPage);

                        // Make the HTTP request
                        var result = await _httpHelper.GetAsync<WoolworthsSearchResult>(
                            finalUrl,
                            headers: headers,
                            cookies: cookies);

                        // Extract product items (empty array if null)
                        var items = result?.Body?.Products.Items ?? [];
                        var itemsx = result?.SetCookies;

                        // If no products returned, stop processing this page
                        if (!items.Any())
                        {
                            return;
                        }

                        // Process each product returned from the API
                        foreach (var item in items)
                        {
                            var price = 0.0;

                            var originalPrice = item.Price?.OriginalPrice ?? 0.0;
                            var salePrice = item.Price?.SalePrice ?? 0.0;

                            var canShowSalePrice = item.Price?.CanShowSalePrice ?? false;
                            var canShowOriginalPrice = item.Price?.CanShowOriginalPrice ?? false;

                            // Pricing rules
                            if (originalPrice > salePrice && canShowSalePrice)
                            {
                                price = salePrice;
                            }
                            else if (canShowOriginalPrice && originalPrice > 0)
                            {
                                price = originalPrice;
                            }

                            // Only add products with a valid price
                            if (price != 0)
                            {
                                // Lock required because List<T> is not thread-safe
                                lock (products)
                                {
                                    products.Add(new Product(
                                        Name: item.Name,
                                        Price: price,
                                        Store: StoreType.Woolworths,
                                        ImageUrl: item.Image?.Big ?? string.Empty,
                                        region));
                                }
                            }
                        }

                        // Respect rate-limiting: wait 300ms after each call
                        await Task.Delay(300);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Error fetching products for category '{category}' page {currentPage}: {ex.Message}");
                        Console.WriteLine($"{url(category, currentPage)}");
                        Console.WriteLine($"aga: {cookies["aga"]}");
                        Console.WriteLine($"session: {cookies["ASP.NET_SessionId"]}");
                    }
                    finally
                    {
                        // Always release the semaphore slot,
                        // even if the request throws an exception
                        semaphore.Release();
                    }
                }));
            }
        }

// Wait until all API calls across all categories/pages are finished
        await Task.WhenAll(tasks);


        return products;
    }
}

/* Example usage:
var url = "https://api.example.com/items";
var payload = new { filter = "fresh" }; // optional
var headers = new Dictionary<string,string> { ["Authorization"] = "Bearer TOKEN" };

var result = await HttpJsonClient.GetJsonAsync<MyResponseDto>(url, payload, headers);
*/