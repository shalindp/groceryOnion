using System.ComponentModel.DataAnnotations;
using Application.Enums;

namespace Presentation.Requests.Product;

public class ProductsPriceRequest
{
    [Required] public Guid ProductId { get; set; }
    [Required] public StoreName StoreName { get; init; }
    [Required] public string StoreId { get; init; }
    [Required] public string StoreSku { get; init; }
}
