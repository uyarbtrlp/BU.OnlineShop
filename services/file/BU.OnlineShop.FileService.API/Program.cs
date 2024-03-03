using Serilog.Events;
using Serilog;
using BU.OnlineShop.Shared.Repository;
using Microsoft.EntityFrameworkCore;
using BU.OnlineShop.FileService.Database.EntityFrameworkCore;
using BU.OnlineShop.FileService.Domain.FileInformations;
using BU.OnlineShop.FileService.Database.FileInformations;
using BU.OnlineShop.Shared.Exceptions;

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

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseErrorHandler();
app.UseHttpsRedirection();
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
