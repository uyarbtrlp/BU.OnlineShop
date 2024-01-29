using BU.OnlineShop.Integration.Messages;

namespace Bu.OnlineShop.BasketService.Abstractions
{
    public class BasketEto : BaseEto
    {
        public Guid UserId { get; set; }

        public double Total { get; set; }

        public List<BasketItemEto> Items { get; set; }
    }
}
