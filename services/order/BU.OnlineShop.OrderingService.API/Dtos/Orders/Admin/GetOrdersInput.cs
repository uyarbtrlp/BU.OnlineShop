using BU.OnlineShop.OrderingService.Orders;

namespace BU.OnlineShop.OrderingService.API.Dtos.Orders.Admin
{
    public class GetOrdersInput
    {
        public Guid? Id { get; set; }

        public Guid? UserId { get; set; }

        public OrderStatus? OrderStatus { get; set; }
    }
}
