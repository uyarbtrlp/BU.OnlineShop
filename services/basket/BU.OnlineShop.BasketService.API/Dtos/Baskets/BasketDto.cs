namespace BU.OnlineShop.BasketService.API.Dtos.Baskets
{
    public class BasketDto
    {
        public float TotalPrice { get; set; }

        public List<BasketLineDto> Items { get; set; }

        public BasketDto()
        {
            Items = new List<BasketLineDto>();
        }
    }
}
