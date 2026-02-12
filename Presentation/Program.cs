using System.Text;
using Application.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        NameClaimType = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName,
        RoleClaimType = System.Security.Claims.ClaimTypes.Role,
    };
});
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IHttpHelper, HttpHelper>();


PersistenceModule.AddToService(builder.Services);
ApplicationModule.AddToService(builder.Services);

builder.Services.AddSingleton<IProductMapper, ProductMapper>();
builder.Services.AddSingleton<IRegionMapper, RegionMapper>();
builder.Services.AddSingleton<IUserMapper, UserMapper>();
builder.Services.AddSingleton<IStoreMapper, StoreMapper>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();