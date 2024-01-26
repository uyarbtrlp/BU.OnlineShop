namespace Bu.OnlineShop.OrderingService.Abstractions
{
    public static class OrderingServiceEventBusConsts
    {
        public static string QueueName = "OrderingService";

        public static string SendOrderRoutingKey = "OrderingService.SendOrder";
    }
}