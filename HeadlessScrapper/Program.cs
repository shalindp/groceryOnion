using HeadlessScrapper.Woolworths;
using PuppeteerSharp;

class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("starting puppeteer...");


        // Open new page
        
        // await page.GoToAsync(WoolworthsProductCrawler.WoolworthsUrl);
        var selectStore = new WoolworthsProductCrawler();
        await selectStore.Start();

        // Navigate to website
        // await page.GoToAsync("https://www.woolworths.co.nz/shop/browse/fruit-veg");
        //
        // // Wait for your custom element
        // await page.WaitForSelectorAsync("product-grid");
        //
        // while (true)
        // {
        //     var productCardsHandle = await page.QuerySelectorAllAsync("cdx-card");
        //
        //     foreach (var productCardHandle in productCardsHandle)
        //     {
        //         var productNameHandle = await productCardHandle.QuerySelectorAsync("[id^='product-'][id$='-title']");
        //
        //         if (productNameHandle == null) break;
        //
        //         var nameText = await productNameHandle.EvaluateFunctionAsync<string>("el => el.innerText");
        //         Console.WriteLine($"Product Name: {nameText}");
        //     }
        //
        //     var nextHandle = await page.QuerySelectorAsync("a[aria-label='Next']");
        //     
        //     if (nextHandle == null) break;
        //     
        //     await nextHandle.ClickAsync();
        // }
    }
}