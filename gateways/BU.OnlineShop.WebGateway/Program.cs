using BU.OnlineShop.WebGateway;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var authServerUrl = configuration["AuthServer:Authority"];

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", true, true);


builder.Services.AddControllers();
builder.Services.AddOcelot();

//var authenticationScheme = "OnlineShopWebGatewayAuthenticationScheme";

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(authenticationScheme,options =>
//    {
//        options.Authority = authServerUrl;
//        options.Audience = "OnlineShop_Swagger";
//    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var authorizationUrl = new Uri($"{authServerUrl}/connect/authorize");
    var tokenUrl = new Uri($"{authServerUrl}/connect/token");

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
                    {"catalogservice.fullaccess", "Catalog Service API"},
                    {"basketservice.fullaccess", "Basket Service API"}
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
if (app.Environment.IsDevelopment())
{
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

                options.SwaggerEndpoint($"{url}/swagger/v1/swagger.json", $"{config.ServiceKey} API");
            }

            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            options.OAuthUsePkce();
        }
    });
}

app.UseCors();

app.UseRewriter(new RewriteOptions()
    // Regex for "", "/" and "" (whitespace)
    .AddRedirect("^(|\\|\\s+)$", "/swagger"));

app.UseOcelot().Wait();

app.UseAuthorization();

app.MapControllers();

app.Run();
