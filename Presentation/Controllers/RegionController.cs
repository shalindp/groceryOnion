using Application.Actions.Regions;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mappers;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class RegionController : ControllerBase
{
    private readonly IWoolworthsRegionAction _woolworthsRegionAction;
    private readonly IRegionMapper _mapper;

    public RegionController(IWoolworthsRegionAction woolworthsRegionAction, IRegionMapper mapper)
    {
        _woolworthsRegionAction = woolworthsRegionAction;
        _mapper = mapper;
    }

    [HttpGet(Name = nameof(GetAllRegions))]
    public async Task<IEnumerable<WoolworthsGetRegionsResult>> GetAllRegions()
    {
        var result = await _woolworthsRegionAction.GetRegionsAsync();
        return result;
    }
}