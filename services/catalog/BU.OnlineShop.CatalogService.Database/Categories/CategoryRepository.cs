using BU.OnlineShop.CatalogService.Database.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BU.OnlineShop.CatalogService.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CatalogServiceDbContext _catalogServiceDbContext;

        public CategoryRepository(CatalogServiceDbContext catalogServiceDbContext)
        {
            _catalogServiceDbContext = catalogServiceDbContext;
        }

        public async Task<Category> InsertAsync(Category category, bool autoSave = false)
        {
            var newCategory = await _catalogServiceDbContext.AddAsync(category);

            if (autoSave)
            {
                await SaveChangesAsync();
            }

            return newCategory.Entity;
        }

        public async Task<IEnumerable<Category>> GetListAsync()
        {
            return await _catalogServiceDbContext.Categories.ToListAsync();
        }

        public async Task<long> GetCountAsync()
        {
            return await _catalogServiceDbContext.Categories.LongCountAsync();
        }
        public async Task<Category> GetAsync(Guid id)
        {
            var category = await _catalogServiceDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            return category;
        }
        public async Task<Category> FindAsync(Guid id)
        {
            return await _catalogServiceDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Category> UpdateAsync(Category Category, bool autoSave = false)
        {
            _catalogServiceDbContext.Attach(Category);

            var updatedCategory = _catalogServiceDbContext.Categories.Update(Category).Entity;

            if (autoSave)
            {
                await SaveChangesAsync();
            }

            return updatedCategory;
        }

        public async Task DeleteAsync(Category Category, bool autoSave = false)
        {
            _catalogServiceDbContext.Categories.Remove(Category);

            if (autoSave)
            {
                await SaveChangesAsync();
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _catalogServiceDbContext.SaveChangesAsync() > 0;
        }
    }
}
