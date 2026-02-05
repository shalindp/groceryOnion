using Application.Actions.Products;
using Application.Commands.Products;
using Application.Enums;
using Application.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mappers;
using Presentation.Responses;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IWoolworthsProductAction _woolworthsProductAction;
    private readonly IProductMapper _mapper;
    private readonly SyncStoreProductsCommand _syncStoreProductsCommand;
    private readonly SyncCanonicalProductsCommand _syncCanonicalProductsCommand;

    public ProductController(IWoolworthsProductAction woolworthsProductAction, IProductMapper mapper,
        SyncStoreProductsCommand syncStoreProductsCommand, SyncCanonicalProductsCommand syncCanonicalProductsCommand)
    {
        _woolworthsProductAction = woolworthsProductAction;
        _mapper = mapper;
        _syncStoreProductsCommand = syncStoreProductsCommand;
        _syncCanonicalProductsCommand = syncCanonicalProductsCommand;
    }

    [HttpGet("categories", Name = nameof(GetCategories))]
    public async Task<IEnumerable<Categoery>> GetCategories()
    {
        var result = await _woolworthsProductAction.GetAllCategoriesAsync();

        return result;
    }

    [HttpPost("sync", Name = nameof(SyncWoolworths))]
    public async Task<bool> SyncWoolworths([FromBody] StoreName[]? stores)
    {
        var result = await _syncStoreProductsCommand.SendAsync(
            new SyncStoreProductsRequest(stores)
        );

        return result;
    }
    
    [HttpPost("sync/canonical", Name = nameof(SyncCanonicalProducts))]
    public async Task<bool> SyncCanonicalProducts()
    {
        var result = await _syncCanonicalProductsCommand.SendAsync();

        return result;
    }

    [HttpGet("search", Name = nameof(SearchProducts))]
    public async Task<IList<ProductResponse>> SearchProducts([FromQuery] string term, int limit, int skip)
    {
        var result = await _woolworthsProductAction.SearchProductsAsync(term, limit, skip);
        return _mapper.Map(result);
    }
}