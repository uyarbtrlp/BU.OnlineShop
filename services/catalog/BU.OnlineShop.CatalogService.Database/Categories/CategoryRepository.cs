using BU.OnlineShop.CatalogService.EntityFrameworkCore;
using BU.OnlineShop.Shared.Repository;
using Microsoft.EntityFrameworkCore;

namespace BU.OnlineShop.CatalogService.Categories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly CatalogServiceDbContext _dbContext;

        public CategoryRepository(CatalogServiceDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Category>> GetListAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<long> GetCountAsync()
        {
            return await _dbContext.Categories.LongCountAsync();
        }
    }
}
