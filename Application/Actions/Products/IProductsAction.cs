using Application.Enums;
using FuzzySharp;
using Persistence;

namespace Application.Actions.Products;

public interface IProductsAction
{
    public Task SyncToCanonicalProducts();
}

public class ProductsAction : IProductsAction
{
    private readonly INpgsqlDbContext _dbContext;

    public ProductsAction(INpgsqlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SyncToCanonicalProducts()
    {
        var woolworthProducts = (await _dbContext.Queries.GetStoreProductsByStore(
            new QueriesSql.GetStoreProductsByStoreArgs()
            {
                StoreName = StoreName.Woolworths.ToDescription(),
            })).Select(c => c.StoreProduct!.Value);

        var paknSaveProducts = (await _dbContext.Queries.GetStoreProductsByStore(
            new QueriesSql.GetStoreProductsByStoreArgs()
            {
                StoreName = StoreName.PaknSave.ToDescription(),
            })).Select(c => c.StoreProduct!.Value).ToList();

        foreach (var woolworthProduct in woolworthProducts)
        {
            StoreProduct? match = paknSaveProducts.FirstOrDefault(c => c.Barcode == woolworthProduct.Barcode);

            if (match.HasValue)
            {
                
            }
        }
    }
}