namespace Bu.OnlineShop.BasketService.Abstractions
{
    public static class BasketServiceEventBusConsts
    {
        public static string QueueName = "BasketService";

        public static string CheckoutRoutingKey = "BasketService.Checkout";
    }
}