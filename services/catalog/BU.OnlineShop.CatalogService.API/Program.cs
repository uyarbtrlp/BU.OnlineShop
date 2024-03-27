using BU.OnlineShop.CatalogService.API;
using BU.OnlineShop.CatalogService.API.MessageBus;
using BU.OnlineShop.CatalogService.Categories;
using BU.OnlineShop.CatalogService.EntityFrameworkCore;
using BU.OnlineShop.CatalogService.Products;
using BU.OnlineShop.Integration.MessageBus;
using BU.OnlineShop.Shared.Exceptions;
using BU.OnlineShop.Shared.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()

#if DEBUG
        .MinimumLevel.Debug()
#else
        .MinimumLevel.Information()
#endif
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
        .Enrich.WithMachineName()
        .Enrich.FromLogContext()
        .WriteTo.File(path: "Logs/logs.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 31, fileSizeLimitBytes: 536870912)
        .WriteTo.Console()
        .CreateLogger();

builder.Host.UseSerilog();

var configuration = builder.Configuration;


Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
// Db setting
builder.Services.AddDbContext<CatalogServiceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("BU.OnlineShop.CatalogService.API")));

// Repository implementation
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Manager implementation
builder.Services.AddTransient<IProductManager, ProductManager>();
builder.Services.AddTransient<ICategoryManager, CategoryManager>();

// Message Bus
builder.Services.AddSingleton<IMessageBus, RabbitMqMessageBus>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddHostedService<MessageBusSubscriber>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = configuration["AuthServer:Authority"];
        options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
        options.MetadataAddress = configuration["AuthServer:MetadataAddress"] + "/" + ".well-known/openid-configuration";
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
        };

        options.TokenValidationParameters.ValidIssuers = new[]
        {
            configuration["AuthServer:Authority"] + "/",
            configuration["AuthServer:MetadataAddress"] + "/",
            };
        options.Audience = "catalogservice";
    });
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers(configure =>
{
    
})
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = ctx => new ValidationResponseHandler();
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var authorizationUrl = new Uri($"{configuration["Swagger:Authority"]}/connect/authorize");
    var tokenUrl = new Uri($"{configuration["Swagger:Authority"]}/connect/token");

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = authorizationUrl,
                Scopes = new
                Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
                {
                    {"catalogservice.fullaccess", "Catalog Service API"}
                },
                TokenUrl = tokenUrl
            }
        }
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
         {
                 {
                     new OpenApiSecurityScheme
                     {
                         Reference = new OpenApiReference
                         {
                             Type = ReferenceType.SecurityScheme,
                             Id = "oauth2"
                         }
                     },
                     Array.Empty<string>()
                 }
         });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog Service API", Version = "v1" });
    options.DocInclusionPredicate((docName, description) => true);
    options.CustomSchemaIds(type => type.FullName);
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .WithOrigins(
                configuration["CorsOrigins"]?
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(o => o.Trim())
                    .ToArray() ?? Array.Empty<string>()
            )
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors();
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog Service API");
    options.OAuthClientId(configuration["Swagger:ClientId"]);
    options.OAuthUsePkce();
});

app.UseErrorHandler();
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Starting BU.OnlineShop.CatalogService.API");
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<CatalogServiceDbContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            Log.Information("Executing database migrations...");
            context.Database.Migrate();
            Log.Information("Database migrations has been completed.");

        }
    }
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly!");
}

finally
{
    Log.CloseAndFlush();
}
