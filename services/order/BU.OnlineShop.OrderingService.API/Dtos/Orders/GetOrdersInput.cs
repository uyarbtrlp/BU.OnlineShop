using BU.OnlineShop.OrderingService.Orders;

namespace BU.OnlineShop.OrderingService.API.Dtos.Orders
{
    public class GetOrdersInput
    {
        public Guid? UserId { get; set; }

        public OrderStatus? OrderStatus { get; set; }
    }
}
