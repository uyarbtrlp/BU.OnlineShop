using BU.OnlineShop.CatalogService.Domain.Repository;

namespace BU.OnlineShop.CatalogService.Categories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetListAsync();

        Task<long> GetCountAsync();
    }
}
