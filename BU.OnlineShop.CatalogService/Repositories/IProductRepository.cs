using BU.OnlineShop.CatalogService.Entities;

namespace BU.OnlineShop.CatalogService.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetListAsync();

        Task<long> GetCountAsync();

        Task<Product> GetAsync(Guid id);

        Task<Product> FindAsync(Guid id);

        Task<Product> FindAsync(string code);

        Task<Product> InsertAsync(Product product, bool autoSave = false);

        Task<Product> UpdateAsync(Product product, bool autoSave = false);

        Task DeleteAsync(Product product, bool autoSave = false);

        Task<bool> SaveChangesAsync();
    }
}
