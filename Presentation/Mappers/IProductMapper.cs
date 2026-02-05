using Application.Models;
using Presentation.Responses;
using Riok.Mapperly.Abstractions;

namespace Presentation.Mappers;

public interface IProductMapper
{
    ProductResponse Map(ProductDto source);
    IList<ProductResponse> Map(IList<ProductDto> source);
}

[Mapper]
public partial class ProductMapper : IProductMapper
{
    public partial ProductResponse Map(ProductDto source);
    public partial IList<ProductResponse> Map(IList<ProductDto> source);
}