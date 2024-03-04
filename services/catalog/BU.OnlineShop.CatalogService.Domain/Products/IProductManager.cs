using JetBrains.Annotations;

namespace BU.OnlineShop.CatalogService.Products
{
    public interface IProductManager
    {
        Task<Product> CreateAsync(
             Guid categoryId,
             Guid imageId,
             string name,
             string code,
             float price,
             int stockCount
            );
         Task<Product> UpdateAsync(
            [NotNull] Product product,
            string name,
            string code,
            float price,
            int stockCount
            );
    }
}
