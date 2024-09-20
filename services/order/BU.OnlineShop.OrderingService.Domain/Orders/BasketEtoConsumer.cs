using AutoMapper;
using Bu.OnlineShop.BasketService.Abstractions;
using BU.OnlineShop.OrderingService.Orders;
using MassTransit;
using System.Text.Json;

namespace BU.OnlineShop.OrderingService.Domain.Orders
{
    public class BasketEtoConsumer : IConsumer<BasketEto>
    {
        private readonly IOrderManager _orderManager;

        private readonly IOrderRepository _orderRepository;

        private readonly IMapper _mapper;

        public BasketEtoConsumer(IOrderManager orderManager, IOrderRepository orderRepository, IMapper mapper)
        {
            _orderManager = orderManager;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<BasketEto> context)
        {
            var order = await _orderManager.CreateAsync(context.Message.UserId, _mapper.Map<List<BasketItemEto>, List<OrderItem>>(context.Message.Items));
            await _orderRepository.InsertAsync(order, true);
        }
    }
}
