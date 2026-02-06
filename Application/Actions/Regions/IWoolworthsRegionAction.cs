using Application.Constants;
using Application.Enums;
using Application.Models;

namespace Application.Actions.Regions;

public interface IWoolworthsRegionAction
{
    public Task<CreateSessionWithRegionDto[]> CreateSessionWithRegionsAsync(int[] addressIds);
    public Task<CreateSessionWithRegionDto> CreateSessionWithRegionAsync(int addressId);
    public Task<IList<WoolworthsGetRegionsResult>> GetRegionsAsync();
}

public record WoolworthsGetRegionsResult(int Id, string StoreName);

public class WoolworthsRegionAction : IWoolworthsRegionAction
{
    private readonly IHttpHelper _httpHelper;

    public WoolworthsRegionAction(IHttpHelper httpHelper)
    {
        _httpHelper = httpHelper;
    }

    public async Task<IList<WoolworthsGetRegionsResult>> GetRegionsAsync()
    {
        const string url = "https://www.woolworths.co.nz/api/v1/addresses/pickup-addresses";
        var result = await _httpHelper.GetAsync<RegionsResponse?>(url, headers: Headers.WoolworthsDefaultHeaders,
            freshSession: true)!;
        var woolworthsGetRegionsResults = result!
            .Body!.StoreAreas.SelectMany(c => c.StoreAddresses)
            .Select(c => new WoolworthsGetRegionsResult(c.Id, c.Name))
            .ToList();

        return woolworthsGetRegionsResults;
    }

    private record RegionsResponse(StoreAreasResponse[] StoreAreas);

    private record StoreAreasResponse(int Id, string Name, StoreAddressesResponse[] StoreAddresses);

    private record StoreAddressesResponse(int Id, string Name);

    public async Task<CreateSessionWithRegionDto[]> CreateSessionWithRegionsAsync(int[] addressIds)
    {
        var tasks = new List<Task<CreateSessionWithRegionDto>>();

        foreach (var regionId in addressIds)
        {
            var task = CreateSessionWithRegionAsync(regionId);
            tasks.Add(task);
        }

        return await Task.WhenAll(tasks);
    }

    public async Task<CreateSessionWithRegionDto> CreateSessionWithRegionAsync(int addressId)
    {
        var url = "https://www.woolworths.co.nz/api/v1/fulfilment/my/pickup-addresses";
        var body = new
        {
            addressId = addressId
        };

        var result = await _httpHelper.PutAsync<ChangeRegionResponse>(url, body,
            headers: Headers.WoolworthsDefaultHeaders, freshSession: true)!;
        var sessionId = _httpHelper.GetCookie(url, result!.Headers, Cookies.AspNetSessionIdCookieName);
        var aga = _httpHelper.GetCookie(url, result!.Headers, Cookies.Aga);

        return new CreateSessionWithRegionDto
        {
            StoreName = StoreName.Woolworths,
            Address = result.Body!.Context.Fulfilment.Address,
            SessionId = sessionId!,
            Aga = aga!
        };
    }


    private record ChangeRegionResponse(ChangeRegionContextResponse Context);

    private record ChangeRegionContextResponse(ChangeRegionFulfillmentResponse Fulfilment);

    private record ChangeRegionFulfillmentResponse(string Address);
}