using AutoMapper;
using BU.OnlineShop.OrderingService.API.Dtos.Orders;
using BU.OnlineShop.OrderingService.API.Dtos.Orders.Admin;
using BU.OnlineShop.OrderingService.API.Permissions;
using BU.OnlineShop.OrderingService.Orders;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BU.OnlineShop.OrderingService.API.Controllers
{
    [Route("api/ordering-service/admin/orders")]
    [ApiController]
    [ProtectedResource(OrderingServicePermissions.GroupName, OrderingServicePermissions.OrderManagementAdmin.Permission)]
    public class OrderAdminController : ControllerBase
    {
        protected IOrderRepository OrderRepository { get; }

        protected IMapper Mapper { get; }

        public OrderAdminController(IOrderRepository orderRepository, IMapper mapper)
        {
            OrderRepository = orderRepository;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<List<OrderDto>> GetlListAsync([FromQuery] Dtos.Orders.Admin.GetOrdersInput input)
        {
            var orders = await OrderRepository.GetListAsync(
                id: input.Id,
                userId: input.UserId,
                orderStatus: input.OrderStatus);

            return Mapper.Map<List<Order>, List<OrderDto>>(orders.ToList());
        }

        [HttpPut]
        [Route("{id}/change-status")]
        [ProtectedResource(OrderingServicePermissions.GroupName, OrderingServicePermissions.OrderManagementAdmin.Update)]
        public async Task<OrderDto> ChangeStatusAsync(Guid id, ChangeStatusInput input)
        {
            var order = await OrderRepository.GetAsync(
                id: id);

            order.SetOrderStatus(input.Status);

            await OrderRepository.UpdateAsync(order, true);

            return Mapper.Map<Order, OrderDto>(order);
        }
    }
}
