using Application.Enums.Products;
using Application.Models.Products;

namespace Application.Products.Actions;

public interface IWoolworthsProductAction : IProductAction
{
}

public class WoolworthsProductAction : IWoolworthsProductAction
{
    private readonly IHttpHelper _httpHelper;

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

    public async Task<IList<Product>> Search(string term)
    {
        const string woolworthsUrl =
            "https://www.woolworths.co.nz/api/v1/products?target=search&search={term}&inStockProductsOnly=false&size=120&page=";
        var url = woolworthsUrl.Replace("{term}", term);

        var headers = new Dictionary<string, string>
        {
            ["User-Agent"] =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3",
            ["x-requested-with"] = "OnlineShopping.WebApp"
        };
        
        var cookies = new Dictionary<string, string>
        {
            { "aga", "766707b394872b181f1414b0000342ea" },
            { "ASP.NET_SessionId", "ji0deyhli4drn1t5auqjduk3" }
        };

        var products = new List<Product>();

        for (var page = 1; page <= 100; page++)
        {
            url = url.Replace("page=", $"page=" + page);

            var result = await _httpHelper.GetAsync<WoolworthsSearchResult>(url, headers: headers, cookies: cookies);

            var items = result?.Products.Items ?? [];
            if (!items.Any())
            {
                break;
            }

            foreach (var item in items)
            {
                var price = 0.0;

                var originalPrice = item.Price?.OriginalPrice ?? 0.0;
                var salePrice = item.Price?.SalePrice ?? 0.0;

                var canShowSalePrice = item.Price?.CanShowSalePrice ?? false;
                var canShowOriginalPrice = item.Price?.CanShowOriginalPrice ?? false;

                if (originalPrice > salePrice && canShowSalePrice)
                {
                    price = salePrice;
                }
                else if (canShowOriginalPrice && originalPrice > 0)
                {
                    price = originalPrice;
                }

                var product = new Product(
                    Name: item.Name,
                    Price: price,
                    Store: StoreType.Woolworths,
                    ImageUrl: item.Image?.Big ?? string.Empty);

                if (price != 0)
                {
                    products.Add(product);
                }
            }
        }

        return products;
    }
}

/* Example usage:
var url = "https://api.example.com/items";
var payload = new { filter = "fresh" }; // optional
var headers = new Dictionary<string,string> { ["Authorization"] = "Bearer TOKEN" };

var result = await HttpJsonClient.GetJsonAsync<MyResponseDto>(url, payload, headers);
*/