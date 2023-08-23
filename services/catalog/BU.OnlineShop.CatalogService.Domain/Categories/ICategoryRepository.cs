namespace BU.OnlineShop.CatalogService.Categories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetListAsync();

        Task<long> GetCountAsync();

        Task<Category> GetAsync(Guid id);

        Task<Category> FindAsync(Guid id);

        Task<Category> InsertAsync(Category Category, bool autoSave = false);

        Task<Category> UpdateAsync(Category Category, bool autoSave = false);

        Task DeleteAsync(Category Category, bool autoSave = false);

        Task<bool> SaveChangesAsync();
    }
}
