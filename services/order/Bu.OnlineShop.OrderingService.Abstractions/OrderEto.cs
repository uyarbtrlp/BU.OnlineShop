namespace Bu.OnlineShop.OrderingService.Abstractions
{
    public class OrderEto
    {
        public Guid UserId { get; set; }

        public double Total { get; set; }

        public List<OrderItemEto> Items { get; set; }
    }
}
