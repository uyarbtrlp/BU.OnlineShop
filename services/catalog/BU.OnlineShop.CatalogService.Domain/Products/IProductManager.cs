namespace BU.OnlineShop.CatalogService.Products
{
    public interface IProductManager
    {
        Task<Product> CreateProductAsync(
             Guid categoryId,
             string name,
             string code,
             float price,
             int stockCount
            );
    }
}
