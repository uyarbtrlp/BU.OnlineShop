using BU.OnlineShop.WebGateway;
using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Authorization;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

builder.Services.AddSingleton<IScopesAuthorizer, CustomScopesAuthorizer>();

var configuration = builder.Configuration;

var authServerUrl = configuration["AuthServer:Authority"];

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);

var authenticationScheme = "OnlineShopWebGatewayAuthenticationScheme";

builder.Services
    .AddKeycloakWebApiAuthentication(builder.Configuration, 
        options => {
            options.TokenValidationParameters.ValidAudiences = new[] {
                "BasketService",
                "CatalogService",
                "OrderingService",
                "FileService"
            };
        }, 
        configSectionName:"Keycloak", jwtBearerScheme: authenticationScheme);


builder.Services.AddControllers();
builder.Services.AddOcelot();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    
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


// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var routes = configuration.GetSection("Routes").Get<List<OcelotConfiguration>>()!;
    var routedServices = routes
        .GroupBy(t => t.ServiceKey)
        .Select(r => r.First())
        .Distinct();

    foreach (var config in routedServices.OrderBy(q => q.ServiceKey))
    {
        if (config.DownstreamHostAndPorts != null)
        {
            var url = $"{config.DownstreamScheme}://{config.DownstreamHostAndPorts.FirstOrDefault()?.Host}:{config.DownstreamHostAndPorts.FirstOrDefault()?.Port}";

            if (!app.Environment.IsDevelopment())
            {
                url = config.SwaggerUrl;
            }

            options.SwaggerEndpoint($"{url}/swagger/v1/swagger.json", $"{config.ServiceKey} API");
        }

        options.OAuthClientId(configuration["Swagger:ClientId"]);
        options.OAuthClientSecret(configuration["Swagger:ClientSecret"]);
        options.OAuthUsePkce();
    }
});

app.UseCors();

app.UseRewriter(new RewriteOptions()
    // Regex for "", "/" and "" (whitespace)
    .AddRedirect("^(|\\|\\s+)$", "/swagger"));

app.UseAuthentication();
app.UseAuthorization();
app.UseOcelot().Wait();

app.MapControllers();

app.Run();
