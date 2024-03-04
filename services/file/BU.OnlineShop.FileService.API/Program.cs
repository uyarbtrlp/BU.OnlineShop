using Serilog.Events;
using Serilog;
using BU.OnlineShop.Shared.Repository;
using Microsoft.EntityFrameworkCore;
using BU.OnlineShop.FileService.Database.EntityFrameworkCore;
using BU.OnlineShop.FileService.Domain.FileInformations;
using BU.OnlineShop.FileService.Database.FileInformations;
using BU.OnlineShop.Shared.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
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

// Db setting
builder.Services.AddDbContext<FileServiceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("BU.OnlineShop.FileService.API")));

// Repository implementation
builder.Services.AddScoped<IFileInformationRepository, FileInformationRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Manager implementation
builder.Services.AddTransient<IFileInformationManager, FileInformationManager>();

var authServerUrl = configuration["AuthServer:Authority"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = authServerUrl;
        options.Audience = "fileservice";
    });
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = ctx => new ValidationResponseHandler();
}); ;
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
                    {"fileservice.fullaccess", "File Service API"}
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
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "File Service API", Version = "v1" });
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "File Service API");
        options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
        options.OAuthUsePkce();
    });
}

app.UseErrorHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

try
{
    Log.Information("Starting BU.OnlineShop.FileService.API");
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
