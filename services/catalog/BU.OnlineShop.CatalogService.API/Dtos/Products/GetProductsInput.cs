namespace BU.OnlineShop.CatalogService.API.Dtos.Products
{
    public class GetProductsInput
    {
        public Guid? CategoryId { get; set; }

        public string Name { get; set; } 

        public string Code { get; set; }

        public float? MinPrice { get; set; }

        public float? MaxPrice { get; set; }

        public int? MinStockCount { get; set; }

        public int? MaxStockCount { get; set; }
    }
}
