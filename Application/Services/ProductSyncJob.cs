using Application.Products.Actions;
using Persistence;
using PuppeteerSharp;

namespace Application.Services;

public sealed class ProductSyncJob
{
    private const string WoolworthsUrl = "https://www.woolworths.co.nz";

    private readonly IHttpHelper _httpClientHelper;
    private readonly INpgsqlDbContext _dbContext;
    private readonly IWoolworthsProductAction _woolworthsProductAction;

    private IPage _page;
    private readonly IList<string> _regionsVisited = new List<string>();
    private readonly Dictionary<string, Cookie> _regionCookies = new();

    private record Cookie(string Session, string Aga);

    public ProductSyncJob(IHttpHelper httpClientHelper, INpgsqlDbContext dbContext,
        IWoolworthsProductAction woolworthsProductAction)
    {
        _httpClientHelper = httpClientHelper;
        _dbContext = dbContext;
        _woolworthsProductAction = woolworthsProductAction;
    }

    public async Task RunAsync(CancellationToken token)
    {
        var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();

        using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true,
            // HeadlessMode = HeadlessMode.True
        });

        using var page = await browser.NewPageAsync();
        await page.SetUserAgentAsync(
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) " +
            "AppleWebKit/537.36 (KHTML, like Gecko) " +
            "Chrome/120.0.0.0 Safari/537.36"
        );


        await page.GoToAsync(WoolworthsUrl);
        _page = page;
        await GoToNextRegion();
    }

    private async Task GoToNextRegion()
    {
        Thread.Sleep(600);
        // Click on the pickup or delivery link
        var pickupOrDeliveryHandle = await _page.WaitForSelectorAsync("a[href='//bookatimeslot']");
        if (pickupOrDeliveryHandle == null)
        {
            throw new Exception("Pickup or Delivery selector not found.");
        }

        await pickupOrDeliveryHandle.ClickAsync();

        // Select the pickup mode
        var pickupMethodHandle = await _page.WaitForSelectorAsync("input[id='method-pickup']");
        if (pickupMethodHandle == null)
        {
            throw new Exception("Pickup method selector not found.");
        }

        await pickupMethodHandle.ClickAsync();

        // Click on the change store button
        var changeStoreButtonHandle = (await _page.XPathAsync(
            "//button[contains(normalize-space(.), 'Change store')]"
        )).FirstOrDefault();
        if (changeStoreButtonHandle == null)
        {
            throw new Exception("Change store button not found.");
        }

        await changeStoreButtonHandle.ClickAsync();

        var regionDropdownHandle = await _page.WaitForSelectorAsync("select[id^='area-dropdown-']");
        if (regionDropdownHandle == null)
        {
            throw new Exception("Region dropdown not found.");
        }

        // Select all regions option
        await regionDropdownHandle.SelectAsync("#area-dropdown-4", "494");

        var regionOptionsHandle = await _page.WaitForSelectorAsync("fulfilment-address-selector");
        if (regionOptionsHandle == null)
        {
            throw new Exception("Region options container not found.");
        }

        await regionOptionsHandle.WaitForSelectorAsync("li");
        var optionsHandles = await regionOptionsHandle.QuerySelectorAllAsync("li");
        Dictionary<string, IElementHandle> d = new();

        foreach (var optionsHandle in optionsHandles)
        {
            await regionOptionsHandle.QuerySelectorAsync("strong");
            var nameHandle = await optionsHandle.QuerySelectorAsync("strong");
            var nameText = await nameHandle.EvaluateFunctionAsync<string>("el => el.innerText");

            var disabledHandle = await optionsHandle.QuerySelectorAsync("button[aria-disabled='true']");
            if (disabledHandle != null)
            {
                continue;
            }

            var buttonHandle = await optionsHandle.QuerySelectorAsync("button");
            // var alreadyVisited = _regionsVisited.Contains(nameText);
            if (buttonHandle == null)
            {
                throw new Exception("Region button not found.");
            }

            d[nameText] = buttonHandle;
        }

        var xx = d.FirstOrDefault(c => !_regionsVisited.Contains(c.Key));
        if (xx.Key == null || xx.Key.Equals("Woolworths Amberley"))
        {
            Console.WriteLine("All regions visited.");
                foreach (var regionCookie in _regionCookies)
                {
                    var products = await _woolworthsProductAction.Search("milk", regionCookie.Value.Session,
                        regionCookie.Value.Aga);

                    var x = products.Select(c => new QueriesSql.CreateProductsArgs()
                    {
                        Name = c.Name,
                        Brand = ""
                    }).ToList();

                    await _dbContext.Queries.CreateProducts(x);
                }
        }
        else
        {
            await xx.Value.ClickAsync();
            _regionsVisited.Add(xx.Key);
            var cookies = await _page.GetCookiesAsync(WoolworthsUrl);
            var sessionCookie = cookies.FirstOrDefault(c => c.Name == "ASP.NET_SessionId");
            var agaCookie = cookies.FirstOrDefault(c => c.Name == "aga");

            _regionCookies[xx.Key] = new Cookie(sessionCookie.Value, agaCookie.Value);
            Console.WriteLine("Selected region: " + xx.Key + " | Session: " + sessionCookie.Value + " | Aga: " +
                              agaCookie.Value);
            await _page.DeleteCookieAsync(await _page.GetCookiesAsync());

            await _page.GoBackAsync();
            await GoToNextRegion();
        }
    }
}