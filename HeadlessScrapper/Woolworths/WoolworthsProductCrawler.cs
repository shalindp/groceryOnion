using System.Text;
using PuppeteerSharp;

namespace HeadlessScrapper.Woolworths;

public class WoolworthsProductCrawler
{
    public static readonly string WoolworthsUrl = "https://www.woolworths.co.nz";

    private readonly IList<string> _regionsVisited = new List<string>();
    private readonly IList<string> _categoriesVisited = new List<string>();
private  IPage _page;
    private readonly IList<string> _categoryBlacklist = new List<string>
    {
        "back-to-school"
    };

    private readonly Dictionary<string, Cookie> _regionCookies = new();

    public record Cookie(string Session, string Aga);

    public async Task Start()
    {
        var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();

        using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true,
            HeadlessMode = HeadlessMode.True
        });
        
        using var page = await browser.NewPageAsync();
        await page.SetUserAgentAsync(
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) " +
            "AppleWebKit/537.36 (KHTML, like Gecko) " +
            "Chrome/120.0.0.0 Safari/537.36"
        );

        _page = page;

        await page.GoToAsync(WoolworthsUrl);
        await GoToNextRegion();
    }

    private async Task GoToNextRegion()
    {
        Thread.Sleep(100);
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

        var regionOptionsHandle = await _page.QuerySelectorAsync("fulfilment-address-selector");
        if (regionOptionsHandle == null)
        {
            throw new Exception("Region options container not found.");
        }

        var optionsHandles = await regionOptionsHandle.QuerySelectorAllAsync("li");
        Dictionary<string, IElementHandle> d = new();

        foreach (var optionsHandle in optionsHandles)
        {
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
        if (xx.Key == null)
        {
            Console.WriteLine("All regions visited.");
            // write all region cookies to csv
            await WriteRegionCookiesToCsvAsync();
        }
        else
        {
            await xx.Value.ClickAsync();
            _regionsVisited.Add(xx.Key);
            var cookies = await _page.GetCookiesAsync(WoolworthsUrl);
            var sessionCookie = cookies.FirstOrDefault(c => c.Name == "ASP.NET_SessionId");
            var agaCookie = cookies.FirstOrDefault(c => c.Name == "aga");

            _regionCookies[xx.Key] = new Cookie(sessionCookie.Value, agaCookie.Value);
            Console.WriteLine("Selected region: " + xx.Key + " | Session: " + sessionCookie.Value + " | Aga: " + agaCookie.Value);
            await _page.DeleteCookieAsync(await _page.GetCookiesAsync());

            // await browser.CloseAsync();
            await _page.GoBackAsync();
            Thread.Sleep(500);
            
            await GoToNextRegion();
        }
    }
    //
    // private async Task<IElementHandle> GetBrowseHandle()
    // {
    //     var browseHandle = await _page.WaitForXPathAsync(
    //         "//nav//span[contains(text(), 'Browse')]"
    //     );
    //     if (browseHandle == null)
    //     {
    //         throw new Exception("Browse link not found.");
    //     }
    //
    //     return browseHandle;
    // }
    //
    // private async Task<IElementHandle[]> GetCategoryMenu()
    // {
    //     var browseHandle = await GetBrowseHandle();
    //     await browseHandle.ClickAsync();
    //
    //     var sideMenuHandle = await _page.WaitForSelectorAsync("global-nav-browse-menu-items");
    //     if (sideMenuHandle == null)
    //     {
    //         throw new Exception("Side menu not found.");
    //     }
    //
    //     var sideMenuItemsHandles = await sideMenuHandle.QuerySelectorAllAsync("li");
    //     if (!sideMenuItemsHandles.Any())
    //     {
    //         throw new Exception("Side menu items not found.");
    //     }
    //
    //     return sideMenuItemsHandles;
    // }
    //
    // private async Task CategoryLoop()
    // {
    //     var sideMenuItemsHandles = await GetCategoryMenu();
    //     foreach (var sideMenuItemsHandle in sideMenuItemsHandles)
    //     {
    //         var linkHandle = await sideMenuItemsHandle.QuerySelectorAsync("a");
    //         if (linkHandle == null)
    //         {
    //             throw new Exception("Side menu link not found.");
    //         }
    //
    //         var href = await linkHandle.EvaluateFunctionAsync<string>(
    //             "el => el.getAttribute('href')"
    //         );
    //
    //         if (string.IsNullOrEmpty(href))
    //         {
    //             throw new Exception("Side menu link href not found.");
    //         }
    //
    //         if (_categoryBlacklist.Any(b => href.Contains(b)) ||
    //             _categoriesVisited.Contains(href))
    //         {
    //             continue;
    //         }
    //
    //         Console.WriteLine("Visiting category: " + href);
    //         await _page.GoToAsync("https://www.woolworths.co.nz" + href);
    //
    //         // Thread.Sleep(1000);
    //         var x = await GetBrowseHandle();
    //         await x.ClickAsync();
    //         // Thread.Sleep(50000);
    //     }
    // }
    
    private async Task WriteRegionCookiesToCsvAsync()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Region,ASP.NET_SessionId,aga");

        foreach (var kv in _regionCookies)
        {
            var region = EscapeCsv(kv.Key);
            var session = EscapeCsv(kv.Value?.Session);
            var aga = EscapeCsv(kv.Value?.Aga);
            sb.AppendLine($"{region},{session},{aga}");
        }

        var path = Path.Combine(Directory.GetCurrentDirectory(), "woolworths_region_cookies.csv");
        await File.WriteAllTextAsync(path, sb.ToString(), Encoding.UTF8);
        Console.WriteLine($"Wrote {_regionCookies.Count} region cookies to {path}");
    }
    
    private static string EscapeCsv(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return "\"\"";
        return "\"" + value.Replace("\"", "\"\"") + "\"";
    }

}