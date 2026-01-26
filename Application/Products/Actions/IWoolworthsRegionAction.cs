using Application.Constants;
using Application.Models;

namespace Application.Products.Actions;

public interface IWoolworthsRegionAction
{
    public Task<Result<ChangeRegionResult?>> CreateSessionWithRegionAsync(int addressId);
}

public record ChangeRegionResult(string Address, string SessionId, string Aga);

public class WoolworthsRegionAction : IWoolworthsRegionAction
{
    private readonly IHttpHelper _httpHelper;

    public WoolworthsRegionAction(IHttpHelper httpHelper)
    {
        _httpHelper = httpHelper;
    }

    public async Task<Result<ChangeRegionResult?>> CreateSessionWithRegionAsync(int addressId)
    {
        const string url = "https://www.woolworths.co.nz/api/v1/fulfilment/my/pickup-addresses";
        var body = new
        {
            addressId
        };

        var headers = new Dictionary<string, string>
        {
            ["User-Agent"] =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3",
            ["x-requested-with"] = "OnlineShopping.WebApp"
        };

        try
        {
            var result = await _httpHelper.PutAsync<ChangeRegionResponse>(url, body, headers: headers)!;
            var sessionId =_httpHelper.GetCookie(url, result!.Headers, Cookies.AspNetSessionIdCookieName);
            var aga = _httpHelper.GetCookie(url, result!.Headers, Cookies.Aga);
            
            return Result<ChangeRegionResult>.Success(new ChangeRegionResult(
                result.Body!.Context.Fulfilment.Address,
                sessionId!,
                aga!
            ));
        }
        catch (Exception ex)
        {
            return Result<ChangeRegionResult>.Failure($"Error changing region: {ex.Message}");
        }
    }

    private record ChangeRegionResponse(ChangeRegionResponseContext Context);

    private record ChangeRegionResponseContext(ChangeRegionFulfillment Fulfilment);

    private record ChangeRegionFulfillment(string Address);
}