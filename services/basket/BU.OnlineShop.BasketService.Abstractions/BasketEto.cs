namespace Bu.OnlineShop.BasketService.Abstractions
{
    public class BasketEto
    {
        public Guid UserId { get; set; }

        public double Total { get; set; }

        public List<BasketItemEto> Items { get; set; }
    }
}
