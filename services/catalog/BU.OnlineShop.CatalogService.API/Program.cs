using BU.OnlineShop.CatalogService.Categories;
using BU.OnlineShop.CatalogService.Domain.Products;
using BU.OnlineShop.CatalogService.EntityFrameworkCore;
using BU.OnlineShop.CatalogService.Products;
using BU.OnlineShop.Shared.Exceptions;
using BU.OnlineShop.Shared.Repository;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
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
        .WriteTo.OpenTelemetry(options =>
        {
            options.ResourceAttributes = new Dictionary<string, object>
            {
                ["service.name"] = "CatalogService"
            }; 
        })  
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
builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<BasketEtoConsumer>();

    busConfigurator.UsingRabbitMq((context, cfg) =>
    {
        cfg.ReceiveEndpoint(e =>
        {
            e.ConfigureConsumer<BasketEtoConsumer>(context);
        });

        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });

        cfg.ConfigureEndpoints(context);
    });

});

builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
builder.Services
    .AddKeycloakAuthorization()
    .AddAuthorizationServer(builder.Configuration);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers(options => options.AddProtectedResources())
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = ctx => new ValidationResponseHandler();
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //var authorizationUrl = new Uri($"{configuration["Swagger:Authority"]}/connect/authorize");
    //var tokenUrl = new Uri($"{configuration["Swagger:Authority"]}/connect/token");
    var authorizationUrl = new Uri($"{configuration["Swagger:Authority"]}/openid-connect/auth");
    var tokenUrl = new Uri($"{configuration["Swagger:Authority"]}/openid-connect/token");


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
                    {"CatalogService", "Catalog Service API"}
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

builder.Services
    .AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("CatalogService"))
    .WithMetrics(metrics =>
    {
        metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation();

        metrics.AddOtlpExporter();
    })
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSqlClientInstrumentation(o => o.SetDbStatementForText = true) // For development purpose
            .AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName);

        tracing.AddOtlpExporter();
    });

builder.Logging.AddOpenTelemetry(logging => {
    logging.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("CatalogService"));
    logging.AddOtlpExporter();
}
);

var app = builder.Build();

app.UseCors();
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog Service API");
    options.OAuthClientId(configuration["Swagger:ClientId"]);
    options.OAuthClientSecret(configuration["Swagger:ClientSecret"]);
    options.OAuthUsePkce();
});

app.UseErrorHandler();
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();

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
