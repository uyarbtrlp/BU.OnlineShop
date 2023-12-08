namespace BU.OnlineShop.BasketService.API.Dtos.Baskets
{
    public class BasketLineDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Count { get; set; }
        public float TotalPrice { get; set; }
    }
}
