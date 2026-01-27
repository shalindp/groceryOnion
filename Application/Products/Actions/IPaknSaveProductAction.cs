// using System.Security.Cryptography;
// using Application.Models;
// using Persistence;
//
// namespace Application.Products.Actions;
//
// public interface IPaknSaveProductAction : IProductAction
// {
// }
//
// public class PaknSaveProductAction : IPaknSaveProductAction
// {
//     private readonly IHttpHelper _httpHelper;
//
//     public PaknSaveProductAction(IHttpHelper httpHelper)
//     {
//         _httpHelper = httpHelper;
//     }
//
//     public async Task<IList<Product>> Search(string term)
//     {
//         const string url = "https://www.paknsave.co.nz/api/user/get-current-user";
//         var response = await _httpHelper.PostAsync<PaknSaveFingerPrintResponse>(url);
//         // var accessToken = response?.access_token;
//         
//         
//
//
//         return [];
//     }
//
//     public static string GenerateRandomHex32()
//     {
//         var bytes = new byte[16];
//         RandomNumberGenerator.Fill(bytes);
//         return Convert.ToHexString(bytes).ToLowerInvariant();
//     }
//
//     private record PaknSaveFingerPrintResponse(string access_token);
//
//     private record PaknSaveFingerPrintRequest(string FingerprintUser, string FingerprintGuest);
//
//     public Task<IList<Product>> Search(string term, string sessionId, string aga)
//     {
//         throw new NotImplementedException();
//     }
// }