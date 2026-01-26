using Application.Models.Products;
using Application.Products.Actions;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class RegionController : ControllerBase
{
    [HttpGet(Name = nameof(GetRegions))]
    public async Task<IEnumerable<Product>> GetRegions()
    {
        var result = await _woolworthsRegionAction.CreateSessionWithRegionAsync(2315277);
        return [];
    } 
    
    
    [HttpGet(Name = nameof(SelectRegion))]
    public async Task<IEnumerable<Product>> SelectRegion([FromQuery(Name = "term")] string term)
    {
        var result = await _woolworthsRegionAction.CreateSessionWithRegionAsync(2315277);
        return [];
    }
    
}