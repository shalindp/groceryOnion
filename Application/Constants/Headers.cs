namespace Application.Constants;

public class Headers
{
    public static Dictionary<string, string> WoolworthsDefaultHeaders = new Dictionary<string, string>
    {
        { "Accept", "application/json" },
        { "User-Agent", "api-client/1.0" },
        { "x-requested-with", "OnlineShopping.WebApp" }
    };
}