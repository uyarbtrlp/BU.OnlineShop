using BU.OnlineShop.CatalogService.Domain.Repository;

namespace BU.OnlineShop.CatalogService.Products
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetListAsync(
            Guid? categoryId = null,
            string name = null,
            string code = null,
            float? minPrice = null,
            float? maxPrice = null,
            int? minStockCount = null,
            int? maxStockCount = null);

        Task<long> GetCountAsync(
            Guid? categoryId = null,
            string name = null,
            string code = null,
            float? minPrice = null,
            float? maxPrice = null,
            int? minStockCount = null,
            int? maxStockCount = null);

        Task<Product> FindAsync(string code);

    }
}
