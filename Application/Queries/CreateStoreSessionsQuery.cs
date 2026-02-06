using Application.Actions.Products;
using Application.Actions.Regions;
using Application.Interfaces;
using Application.Models;

namespace Application.Queries;

public record CreateStoreSessionsQueryRequest
{
    public int[] WoolworthsStoresAddressIds { get; init; }
    public bool ShouldCreateForPaknSave { get; init; }
}

public record CreateStoreSessionsQueryResponse
{
    public CreateSessionWithRegionDto[] WoolworthsSessions { get; init; }
    public string AccessToken { get; init; }
}

public class CreateStoreSessionsQuery : IQuery<CreateStoreSessionsQueryResponse, CreateStoreSessionsQueryRequest>
{
    private readonly IWoolworthsProductAction _woolworthsProductAction;
    private readonly IPaknSaveProductAction _paknSaveProductAction;
    private readonly IWoolworthsRegionAction _woolworthsRegionAction;

    public CreateStoreSessionsQuery(IWoolworthsProductAction woolworthsProductAction,
        IPaknSaveProductAction paknSaveProductAction, IWoolworthsRegionAction woolworthsRegionAction)
    {
        _woolworthsProductAction = woolworthsProductAction;
        _paknSaveProductAction = paknSaveProductAction;
        _woolworthsRegionAction = woolworthsRegionAction;
    }

    public async Task<CreateStoreSessionsQueryResponse> SendAsync(CreateStoreSessionsQueryRequest queryRequestBody)
    {
        var woolworthsSessionRegions =
            await _woolworthsRegionAction.CreateSessionWithRegionsAsync(queryRequestBody.WoolworthsStoresAddressIds);
        
        var paknSaveAccessToken = await _paknSaveProductAction.CreateAccessTokenAsync();
        
        return new CreateStoreSessionsQueryResponse
        {
            WoolworthsSessions = woolworthsSessionRegions,
            AccessToken = paknSaveAccessToken
        };
    }
}