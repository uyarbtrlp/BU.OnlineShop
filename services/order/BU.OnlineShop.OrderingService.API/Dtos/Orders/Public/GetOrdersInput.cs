using BU.OnlineShop.OrderingService.Orders;
using System.ComponentModel.DataAnnotations;

namespace BU.OnlineShop.OrderingService.API.Dtos.Orders
{
    public class GetOrdersInput
    {
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus? OrderStatus { get; set; }
    }
}
