using BU.OnlineShop.Integration.Messages;

namespace Bu.OnlineShop.OrderingService.Abstractions
{
    public class OrderEto : BaseEto
    {
        public Guid UserId { get; set; }

        public double Total { get; set; }

        public List<OrderItemEto> Items { get; set; }
    }
}
