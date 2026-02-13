using Application.Constants;
using Application.Enums;
using Application.Queries.Store;

namespace Application.Actions.Regions;

public interface IWoolworthsStoreAction
{
    public Task<IList<StoreQueryResponse>> GetStoresAsync();
}

public record WoolworthsGetRegionsResult(string StoreId, string StoreName);

    public class WoolworthsStoreAction : IWoolworthsStoreAction
    {
        private readonly IHttpHelper _httpHelper;

        public WoolworthsStoreAction(IHttpHelper httpHelper)
        {
            _httpHelper = httpHelper;
        }

        public async Task<IList<StoreQueryResponse>> GetStoresAsync()
        {
            const string url = "https://www.woolworths.co.nz/api/v1/addresses/pickup-addresses";
            var result = await _httpHelper.GetAsync<RegionsResponse?>(url, headers: Headers.WoolworthsDefaultHeaders);
            var woolworthsGetRegionsResults = result!
                .Body!.StoreAreas.SelectMany(c => c.StoreAddresses)
                .Select(c => new WoolworthsGetRegionsResult(c.Id.ToString(), c.Name))
                .DistinctBy(c => c.StoreId)
                .ToList();

            return woolworthsGetRegionsResults.Select(c => new StoreQueryResponse
            {
                StoreId = c.StoreId,
                StoreRegionName = c.StoreName,
                StoreName = StoreName.Woolworths,
            }).ToList();
        }

        private record RegionsResponse(StoreAreasResponse[] StoreAreas);

        private record StoreAreasResponse(int Id, string Name, StoreAddressesResponse[] StoreAddresses);

        private record StoreAddressesResponse(int Id, string Name);
    }