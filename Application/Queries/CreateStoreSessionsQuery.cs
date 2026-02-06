using Application.Actions.Products;
using Application.Actions.Regions;
using Application.Interfaces;
using Application.Models;

namespace Application.Queries;

public record CreateStoreSessionsQueryRequest(int[] WoolworthsStoresAddressIds, bool ShouldCreateForPaknSave);

public record CreateStoreSessionsQueryResponse(CreateSessionWithRegionDto[] WoolworthsSessions, string AccessToken);

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
        
        return new CreateStoreSessionsQueryResponse(woolworthsSessionRegions, paknSaveAccessToken);
    }
}