using BU.OnlineShop.OrderingService.Orders;
using System.ComponentModel.DataAnnotations;

namespace BU.OnlineShop.OrderingService.API.Dtos.Orders.Admin
{
    public class ChangeStatusInput
    {
        [Required]
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus Status { get; set; } = 0;
    }
}
