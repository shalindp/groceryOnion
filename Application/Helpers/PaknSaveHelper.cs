using System.Security.Cryptography;

namespace Application.Helpers;

public class PaknSaveHelper
{
    public static Dictionary<string, string> BuildAuthenticationHeader(string accessToken)
    {
        return new Dictionary<string, string>()
        {
            ["authorization"] = $"Bearer {accessToken}"
        };
    }

    public static string GenerateRandomHex32()
    {
        var bytes = new byte[16];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}