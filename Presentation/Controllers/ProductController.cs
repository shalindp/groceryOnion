using Application.Models.Products;
using Application.Products.Actions;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    
    private readonly IWoolworthsProductAction _woolworthsProductAction;
    private readonly IPaknSaveProductAction _paknSaveProductAction;

    public ProductController(IWoolworthsProductAction woolworthsProductAction, IPaknSaveProductAction paknSaveProductAction)
    {
        _woolworthsProductAction = woolworthsProductAction;
        _paknSaveProductAction = paknSaveProductAction;
    }

    [HttpGet(Name = nameof(Search))]
    public async Task<IEnumerable<Product>> Search([FromQuery(Name = "term")] string term)
    {
        // IList<Product> products = new List<Product>();

        // var p = await _woolworthsProductAction.Search(term);
        // return p;
        // return await _paknSaveProductAction.Search(term);
        return [];
    }
}