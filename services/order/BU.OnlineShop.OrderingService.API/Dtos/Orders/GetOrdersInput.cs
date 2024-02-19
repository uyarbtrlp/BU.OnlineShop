using BU.OnlineShop.OrderingService.Orders;

namespace BU.OnlineShop.OrderingService.API.Dtos.Orders
{
    public class GetOrdersInput
    {
        public OrderStatus? OrderStatus { get; set; }
    }
}
