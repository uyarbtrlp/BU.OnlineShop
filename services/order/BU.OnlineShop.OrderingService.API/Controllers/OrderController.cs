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

        public OrderController(IOrderRepository orderRepository)
        {
            OrderRepository = orderRepository;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<List<OrderDto>> GetListByUserId(Guid userId)
        {
            var orders = await OrderRepository.GetListByUserId(userId);

            return GetOrderDtoMapping(orders.ToList());
        }

        private List<OrderDto> GetOrderDtoMapping(List<Order> orders)
        {
            List<OrderDto> dtoList = new List<OrderDto>();

            foreach (var order in orders) {

                dtoList.Add(new OrderDto()
                {
                    UserId = order.UserId,
                    OrderStatus = order.OrderStatus,
                    OrderItems = Mapper.Map<List<OrderItem>, List<OrderItemDto>>(order.OrderItems)
                });
            }

            return dtoList;
        }
    }
}
