using AutoMapper;
using BU.OnlineShop.OrderingService.API.Dtos.Orders;
using BU.OnlineShop.OrderingService.Orders;

namespace BU.OnlineShop.OrderingService.API
{
    public class OrderServiceProfile : Profile
    {
        public OrderServiceProfile()
        {
            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderItemDto>();
        }
    }
}
