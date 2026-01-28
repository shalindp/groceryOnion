using Application.Actions;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IWoolworthsProductAction _woolworthsProductAction;

    public ProductController(IWoolworthsProductAction woolworthsProductAction)
    {
        _woolworthsProductAction = woolworthsProductAction;
    }

    [HttpGet("categories", Name = nameof(GetCategories))]
    public async Task<IEnumerable<Categoery>> GetCategories()
    {
        var result = await _woolworthsProductAction.GetAllCategoriesAsync();

        return result;
    }

    [HttpGet("sync/woolworths", Name = nameof(SyncWoolworths))]
    public async Task<bool> SyncWoolworths()
    {
        await _woolworthsProductAction.SyncProductsAsync();

        return true;
    }

    [HttpGet("search{regionId:int}", Name = nameof(SearchProducts))]
    public async Task<bool> SearchProducts(int regionId)
    {
        await _woolworthsProductAction.SearchProductsAsync("sweet",
            [1225718, 3496448, 861615, 2810973, 1050811, 1155526]);

        return true;
    }
}