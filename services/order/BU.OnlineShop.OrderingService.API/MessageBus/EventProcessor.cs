using AutoMapper;
using Bu.OnlineShop.OrderingService.Abstractions;
using BU.OnlineShop.OrderingService.Orders;
using System.Text.Json;

namespace BU.OnlineShop.OrderingService.API.MessageBus
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper )
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }

        public async Task ProcessEventAsync(string message, string routingKey)
        {
            // Add the order to db
            if(routingKey == OrderingServiceEventBusConsts.SendOrderRoutingKey)
            {
                using(var scope = _serviceScopeFactory.CreateScope())
                {
                    var orderManager = scope.ServiceProvider.GetService<IOrderManager>();
                    var orderRepository = scope.ServiceProvider.GetService<IOrderRepository>();

                    var orderEto = JsonSerializer.Deserialize<OrderEto>(message);
                    var order = await orderManager.CreateAsync(orderEto.UserId, _mapper.Map<List<OrderItemEto>, List<OrderItem>>(orderEto.Items));
                    await orderRepository.InsertAsync(order, true);
                }
            }
        }
    }
}
