using Application.Actions;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class RegionController : ControllerBase
{
    private readonly IWoolworthsRegionAction _woolworthsRegionAction;

    public RegionController(IWoolworthsRegionAction woolworthsRegionAction)
    {
        _woolworthsRegionAction = woolworthsRegionAction;
    }

    [HttpGet("all", Name = nameof(GetAllRegions))]
    public async Task<IEnumerable<WoolworthsGetRegionsResult>> GetAllRegions()
    {
        var result = await _woolworthsRegionAction.GetRegionsAsync();
        return result;
    }


    [HttpGet("select/region/{id:int}")]
    public async Task<WoolworthsChangeRegionResult> SelectRegion(int id)
    {
        // var result = await _woolworthsRegionAction.CreateSessionWithRegionAsync(id);

        
        return null;
    }
}