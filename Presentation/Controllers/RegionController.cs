using System.ComponentModel.DataAnnotations;
using Application.Actions;
using Application.Actions.Regions;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mappers;
using Presentation.Requests;
using Presentation.Responses;
using Swashbuckle.AspNetCore.Annotations;

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

    [HttpPost("create-session", Name = nameof(CreateSessionWithRegionsAsync))]
    [ProducesResponseType(typeof(IList<CreateSessionWithRegionResponse>), StatusCodes.Status200OK)]
    public async Task<IList<CreateSessionWithRegionResponse>> CreateSessionWithRegionsAsync(
        [FromBody] [Required] IList<CreateSessionWithRegionId> request)
    {
        var result =
            await _woolworthsRegionAction.CreateSessionWithRegionsAsync(request.Select(c => c.RegionId).ToArray());
        return _mapper.Map(result);
    }
}