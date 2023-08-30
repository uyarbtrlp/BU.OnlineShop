using BU.OnlineShop.CatalogService.Categories;

namespace BU.OnlineShop.CatalogService.Domain.Categories
{
    public interface ICategoryManager
    {
        Task<Category> CreateAsync(
            string name,
            string description = null);
    }
}
