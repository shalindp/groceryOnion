using Application.Models.Products;

namespace Application.Products.Actions;

public interface IProductAction
{
    public Task<IList<Product>> Search(string term);
}

