using Application.Enums;
using Persistence;

namespace Application.Actions.Products;

public interface ICanonicalProductSyncAction
{
    public Task<List<QueriesSql.CreateCanonicalStoreProductsArgs>> BuildToCanonicalProductsAsync(INpgsqlDbContext dbContext);
}

public class CanonicalProductSyncAction : ICanonicalProductSyncAction
{
    void AddToMap(Dictionary<string, List<StoreProduct>> barcodeMap, List<StoreProduct> products)
    {
        foreach (var product in products)
        {
            if (string.IsNullOrWhiteSpace(product.Barcode))
                continue;

            if (!barcodeMap.TryGetValue(product.Barcode, out var list))
            {
                list = new List<StoreProduct>();
                barcodeMap[product.Barcode] = list;
            }

            list.Add(product);
        }
    }

    public async Task<List<QueriesSql.CreateCanonicalStoreProductsArgs>> BuildToCanonicalProductsAsync(INpgsqlDbContext dbContext)
    {
        var woolworthProducts = (await dbContext.Queries.GetStoreProductsByStore(
            new QueriesSql.GetStoreProductsByStoreArgs()
            {
                StoreName = StoreName.Woolworths.ToDescription(),
            })).Select(c => c.StoreProduct!.Value).ToList();

        var paknSaveProducts = (await dbContext.Queries.GetStoreProductsByStore(
            new QueriesSql.GetStoreProductsByStoreArgs()
            {
                StoreName = StoreName.PaknSave.ToDescription(),
            })).Select(c => c.StoreProduct!.Value).ToList();

        var barcodeMap = new Dictionary<string, List<StoreProduct>>();


        AddToMap(barcodeMap, woolworthProducts);
        AddToMap(barcodeMap, paknSaveProducts);

        var mutableCanonicalStoreProductsArgs =
            new Dictionary<string, List<MutableCreateCanonicalStoreProducts>>();
        var createCanonicalProductArgs = new List<QueriesSql.CreateCanonicalProductsArgs>();

        foreach (var kvp in barcodeMap)
        {
            var barcode = kvp.Key;
            var products = kvp.Value;

            var createCanonicalProductArg = new QueriesSql.CreateCanonicalProductsArgs()
            {
                Barcode = barcode,
                Name = products.FirstOrDefault().Name,
                Brand = products.FirstOrDefault().Brand ?? "",
                ImageUrl = products.FirstOrDefault().ImageUrl,
                MaxQuantity = products.FirstOrDefault().MaxQuantity,
            };

            createCanonicalProductArgs.Add(createCanonicalProductArg);

            mutableCanonicalStoreProductsArgs[barcode] = products.Select(c =>
                new MutableCreateCanonicalStoreProducts(c.StoreProductId)).ToList();
        }

        await dbContext.Queries.CreateCanonicalProducts(createCanonicalProductArgs);

        var barcodes = mutableCanonicalStoreProductsArgs.Keys.ToArray();

        var canonicalProducts = (await dbContext.Queries.GetCanonicalProducts(new QueriesSql.GetCanonicalProductsArgs
        {
            Barcodes = barcodes,
        })).Select(c => c.Product!.Value).ToList();

        foreach (var canonicalProduct in canonicalProducts)
        {
            mutableCanonicalStoreProductsArgs[canonicalProduct.Barcode].ForEach(c => { c.CanonicalProductId = canonicalProduct.ProductId; });
        }

        var createCanonicalStoreProductsArgs = mutableCanonicalStoreProductsArgs.SelectMany(c => c.Value)
            .Select(c => new QueriesSql.CreateCanonicalStoreProductsArgs
            {
                ProductId = c.CanonicalProductId,
                StoreProductId = c.StoreProductId,
            }).ToList();
        
        return createCanonicalStoreProductsArgs;
    }

    private record MutableCreateCanonicalStoreProducts(Guid StoreProductId)
    {
        public Guid CanonicalProductId { get; set; }
    };
}