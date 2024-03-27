using AutoMapper;
using BU.OnlineShop.OrderingService.API.Dtos.Orders;
using BU.OnlineShop.OrderingService.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BU.OnlineShop.OrderingService.API.Controllers
{
    [Route("api/ordering-service/orders")]
    [ApiController]
    [Authorize(Roles = "User,Admin")]
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
            var userId = new Guid(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var orders = await OrderRepository.GetListAsync(
                userId: userId,
                orderStatus: input.OrderStatus);

            return Mapper.Map<List<Order>, List<OrderDto>>(orders.ToList());
        }
    }
}
