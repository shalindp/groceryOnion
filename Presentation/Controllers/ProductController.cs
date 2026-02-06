using Application.Actions.Products;
using Application.Commands.Products;
using Application.Enums;
using Application.Models;
using Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mappers;
using Presentation.Requests.Product;
using Presentation.Responses;
using Presentation.Responses.Product;

namespace Presentation.Controllers;

[ApiController]
// [Authorize]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IWoolworthsProductAction _woolworthsProductAction;
    private readonly IProductMapper _mapper;
    private readonly SyncStoreProductsCommand _syncStoreProductsCommand;
    private readonly SyncCanonicalProductsCommand _syncCanonicalProductsCommand;
    private readonly SearchProductsQuery _searchProductsQuery;
    private readonly GetProductsPricingQuery _getProductsPricingQuery;

    public ProductController(IWoolworthsProductAction woolworthsProductAction, IProductMapper mapper,
        SyncStoreProductsCommand syncStoreProductsCommand, SyncCanonicalProductsCommand syncCanonicalProductsCommand,
        SearchProductsQuery searchProductsQuery, GetProductsPricingQuery getProductsPricingQuery)
    {
        _woolworthsProductAction = woolworthsProductAction;
        _mapper = mapper;
        _syncStoreProductsCommand = syncStoreProductsCommand;
        _syncCanonicalProductsCommand = syncCanonicalProductsCommand;
        _searchProductsQuery = searchProductsQuery;
        _getProductsPricingQuery = getProductsPricingQuery;
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
            new SyncStoreProductsRequest { Stores = stores }
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
        var result = await _searchProductsQuery.SendAsync(new SearchProductsQueryRequest { Term = term, Limit = limit, Skip = skip });
        return _mapper.Map(result.Data!.Products);
    }
    
    [HttpPost("price", Name = nameof(ProductPriceAsync))]
    public async Task<GetProductsPricingResponse> ProductPriceAsync([FromBody] GetProductsPricingRequest request)
    {
        var result = await _getProductsPricingQuery.SendAsync(_mapper.Map(request));
        return _mapper.Map(result);
    }
}