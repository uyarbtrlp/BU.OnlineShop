namespace BU.OnlineShop.BasketService.API.Dtos.CatalogService
{
    public class ProductDto
    {
        public Guid Id { get; set; }

        public Guid CategoryId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public float Price { get; set; }

        public int StockCount { get; set; }
    }
}
