using BU.OnlineShop.BasketService.API.Services;
using BU.OnlineShop.BasketService.Baskets;
using BU.OnlineShop.BasketService.EntityFrameworkCore;
using BU.OnlineShop.Integration.MessageBus;
using BU.OnlineShop.Shared.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Db setting
builder.Services.AddDbContext<BasketServiceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("BU.OnlineShop.BasketService.API")));

// Repository implementation
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Manager implementation
builder.Services.AddTransient<IBasketManager, BasketManager>();

// RemoteService implementation
builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["RemoteServices:CatalogService:Uri"]));

builder.Services.AddHttpClient<IPaymentService, PaymentService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["RemoteServices:PaymentService:Uri"]));

// Message Bus
builder.Services.AddSingleton<IMessageBus, RabbitMqMessageBus>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
