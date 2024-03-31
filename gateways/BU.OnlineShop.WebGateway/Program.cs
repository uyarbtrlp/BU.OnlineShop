using BU.OnlineShop.WebGateway;
using Microsoft.AspNetCore.Rewrite;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var authServerUrl = configuration["AuthServer:Authority"];

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);


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
        options.OAuthUsePkce();
    }
});

app.UseCors();

app.UseRewriter(new RewriteOptions()
    // Regex for "", "/" and "" (whitespace)
    .AddRedirect("^(|\\|\\s+)$", "/swagger"));

app.UseOcelot().Wait();

app.UseAuthorization();

app.MapControllers();

app.Run();
