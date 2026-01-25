using Application.Products.Actions;
using Application.Services;
using Presentation.Mappers;
using Microsoft.OpenApi;
using Persistence.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Some API v1", Version = "v1" });
    // some other configs
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

// Register application services
PersistenceModule.AddToService(builder.Services);

builder.Services.AddHttpClient<IHttpHelper, HttpHelper>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("User-Agent", "MyApi/1.0");
});


builder.Services.AddTransient<IPaknSaveProductAction, PaknSaveProductAction>();
builder.Services.AddTransient<IWoolworthsProductAction, WoolworthsProductAction>();
builder.Services.AddSingleton<IProductMapper, ProductMapper>();

builder.Services.AddHostedService<ProductSyncBackgroundService>();
builder.Services.AddScoped<ProductSyncJob>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();