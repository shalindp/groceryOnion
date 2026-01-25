using Riok.Mapperly.Abstractions;

namespace Presentation.Mappers;

public interface IProductMapper
{
    // Product Map(WoolworthsItem source);
    // IList<Product> Map(IList<WoolworthsItem> source);
    
    // Product Map(WoolworthsProductService.WoolworthsProduct source);
}

[Mapper]
public partial class ProductMapper : IProductMapper
{
    // public partial Product Map(WoolworthsItem source);
    // public partial IList<Product> Map(IList<WoolworthsItem> source);
}