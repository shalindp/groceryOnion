using Application.Settings;
using Presentation.Mappers;
using Microsoft.OpenApi;
using Persistence.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Some API v1", Version = "v1" });
});

// builder.Services.AddHttpClient<IHttpHelper, HttpHelper>(client =>
// {
//     client.Timeout = TimeSpan.FromSeconds(30);
//     client.DefaultRequestHeaders.Add("Accept", "application/json");
//     client.DefaultRequestHeaders.Add("User-Agent", "api-client/1.0");
//     
//     // woolworths specific headers
//     client.DefaultRequestHeaders.Add("x-requested-with", "OnlineShopping.WebApp");
// });

builder.Services.AddScoped<IHttpHelper, HttpHelper>();


PersistenceModule.AddToService(builder.Services);
ApplicationModule.AddToService(builder.Services);

builder.Services.AddSingleton<IProductMapper, ProductMapper>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();