using Application.Actions.Products;
using Application.Actions.Regions;
using Application.Actions.User;
using Application.Commands.Products;
using Application.Commands.Stores;
using Application.Commands.User;
using Application.Queries;
using Application.Queries.User;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Settings;

public static class ApplicationModule
{
    public static void AddToService(IServiceCollection services)
    {
        services.AddSingleton<ICacheService, CacheService>();
        services.AddScoped<JwtAuthenticationService>();

        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IWoolworthsRegionAction, WoolworthsRegionAction>();
        services.AddScoped<IWoolworthsProductAction, WoolworthsProductAction>();
        services.AddScoped<IPaknSaveProductAction, PaknSaveProductAction>();
        services.AddScoped<ICanonicalProductSyncAction, CanonicalProductSyncAction>();

        services.AddScoped<SignInQuery>();
        services.AddScoped<CreateUserCommand>();
        services.AddScoped<SyncStoreProductsCommand>();
        services.AddScoped<SyncCanonicalProductsCommand>();
        services.AddScoped<SearchProductsQuery>();
        services.AddScoped<CreateStoreSessionsQuery>();
        services.AddScoped<RefreshTokenQuery>();
        services.AddScoped<SelectStoresCommand>();
        services.AddScoped<GetProductsPricingQuery>();
    }
}