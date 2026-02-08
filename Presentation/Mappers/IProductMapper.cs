using Application.Models;
using Application.Queries;
using Application.Queries.Product;
using Presentation.Requests.Product;
using Presentation.Responses;
using Presentation.Responses.Product;
using Riok.Mapperly.Abstractions;

namespace Presentation.Mappers;

public interface IProductMapper
{
    ProductResponse Map(ProductDto source);
    ProductPriceQueryRequest[] Map(ProductsPriceRequest[] source);
    ProductsPriceResponse[] Map(ProductPriceQueryRequest[] source);
    IList<ProductResponse> Map(IList<ProductDto> source);
}

[Mapper]
public partial class ProductMapper : IProductMapper
{
    public partial ProductResponse Map(ProductDto source);
    public partial ProductPriceQueryRequest[] Map(ProductsPriceRequest[] source);

    public partial ProductsPriceResponse[] Map(ProductPriceQueryRequest[] source);

    public partial IList<ProductResponse> Map(IList<ProductDto> source);
}