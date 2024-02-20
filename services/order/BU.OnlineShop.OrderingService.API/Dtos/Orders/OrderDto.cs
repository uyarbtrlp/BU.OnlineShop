using BU.OnlineShop.OrderingService.Orders;

namespace BU.OnlineShop.OrderingService.API.Dtos.Orders
{

    public class OrderDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}
