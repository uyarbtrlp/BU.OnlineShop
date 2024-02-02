using AutoMapper;
using Bu.OnlineShop.BasketService.Abstractions;
using BU.OnlineShop.OrderingService.Orders;
using System.Text.Json;

namespace BU.OnlineShop.OrderingService.API.MessageBus
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
                // Add the order to db
                if (routingKey == BasketServiceEventBusConsts.CheckoutRoutingKey)
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var orderManager = scope.ServiceProvider.GetService<IOrderManager>();
                        var orderRepository = scope.ServiceProvider.GetService<IOrderRepository>();

                        var basketEto = JsonSerializer.Deserialize<BasketEto>(message);
                        var order = await orderManager.CreateAsync(basketEto.UserId, _mapper.Map<List<BasketItemEto>, List<OrderItem>>(basketEto.Items));
                        await orderRepository.InsertAsync(order, true);
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
