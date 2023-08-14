using BU.OnlineShop.CatalogService.Entities;

namespace BU.OnlineShop.CatalogService.Services
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
