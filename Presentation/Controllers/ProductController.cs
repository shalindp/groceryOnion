// using Application.Actions.Region;
// using Application.Models.Products;
// using Application.Products.Actions;
// using Microsoft.AspNetCore.Mvc;
//
// namespace Presentation.Controllers;
//
// [ApiController]
// [Route("[controller]")]
// public class ProductController : ControllerBase
// {
//     
//     private readonly IWoolworthsProductAction _woolworthsProductAction;
//     private readonly IPaknSaveProductAction _paknSaveProductAction;
//     private readonly IWoolworthsRegionAction _woolworthsRegionAction;
//
//     public ProductController(IWoolworthsProductAction woolworthsProductAction, IPaknSaveProductAction paknSaveProductAction, IWoolworthsRegionAction woolworthsRegionAction)
//     {
//         _woolworthsProductAction = woolworthsProductAction;
//         _paknSaveProductAction = paknSaveProductAction;
//         _woolworthsRegionAction = woolworthsRegionAction;
//     }
//     
//     // [HttpGet(Name = nameof(SelectRegion))]
//     // public async Task<IEnumerable<Product>> SelectRegion([FromQuery(Name = "term")] string term)
//     // {
//     //     var result = await _woolworthsRegionAction.CreateSessionWithRegionAsync(2315277);
//     //     return [];
//     // }
//     
// }