using BU.OnlineShop.CatalogService.Categories;

namespace BU.OnlineShop.CatalogService.Categories
{
    public class CategoryManager : ICategoryManager
    {
        public Task<Category> CreateAsync(string name, string description = null)
        {
            var category = new Category(
                Guid.NewGuid(),
                name, 
                description
                ); 

            return Task.FromResult(category);
        }
    }
}
