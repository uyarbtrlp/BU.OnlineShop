using AutoMapper;
using Bu.OnlineShop.BasketService.Abstractions;
using BU.OnlineShop.CatalogService.Products;
using System.Text.Json;

namespace BU.OnlineShop.CatalogService.API.MessageBus
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<EventProcessor> _logger;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper, ILogger<EventProcessor> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task ProcessEventAsync(string message, string routingKey)
        {
            try
            {
                // Update the product
                if (routingKey == BasketServiceEventBusConsts.CheckoutRoutingKey)
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var productManager = scope.ServiceProvider.GetService<IProductManager>();
                        var productRepository = scope.ServiceProvider.GetService<IProductRepository>();

                        var basketEto = JsonSerializer.Deserialize<BasketEto>(message);

                        foreach (var basketItem in basketEto.Items)
                        {
                            var product = await productRepository.GetAsync(basketItem.ProductId);

                            product.SetStockCount(product.StockCount - basketItem.Count);

                            await productRepository.UpdateAsync(product, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error has been occurred while processing the event : ${ex.Message}");
            }

        }
    }
}
