namespace BU.OnlineShop.CatalogService.Models.Products
{
    public class ProductDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public float Price { get; set; }

        public int StockCount { get; set; }
    }
}
