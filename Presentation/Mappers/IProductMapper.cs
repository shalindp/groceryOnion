using Application.Models;
using Application.Queries;
using Presentation.Requests.Product;
using Presentation.Responses;
using Presentation.Responses.Product;
using Riok.Mapperly.Abstractions;

namespace Presentation.Mappers;

public interface IProductMapper
{
    ProductResponse Map(ProductDto source);
    GetProductsPricingQueryRequest Map(GetProductsPricingRequest source);
    GetProductsPricingResponse Map(GetProductsPricingQueryResponse source);
    IList<ProductResponse> Map(IList<ProductDto> source);
}

[Mapper]
public partial class ProductMapper : IProductMapper
{
    public partial ProductResponse Map(ProductDto source);
    public partial GetProductsPricingQueryRequest Map(GetProductsPricingRequest source);

    public partial GetProductsPricingResponse Map(GetProductsPricingQueryResponse source);

    public partial IList<ProductResponse> Map(IList<ProductDto> source);
}