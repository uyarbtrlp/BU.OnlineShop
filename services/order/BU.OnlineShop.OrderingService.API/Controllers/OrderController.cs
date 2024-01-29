using AutoMapper;
using BU.OnlineShop.OrderingService.API.Dtos.Orders;
using BU.OnlineShop.OrderingService.Orders;
using Microsoft.AspNetCore.Mvc;

namespace BU.OnlineShop.OrderingService.API.Controllers
{
    [Route("api/ordering-service/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        protected IOrderRepository OrderRepository { get; }

        protected IMapper Mapper { get; }

        public OrderController(IOrderRepository orderRepository, IMapper mapper)
        {
            OrderRepository = orderRepository;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<List<OrderDto>> GetlListAsync([FromQuery] GetOrdersInput input)
        {
            var orders = await OrderRepository.GetListAsync(
                userId: input.UserId,
                orderStatus: input.OrderStatus);

            return Mapper.Map<List<Order>, List<OrderDto>>(orders.ToList());
        }

        //TODO: look the product stock update
    }
}
