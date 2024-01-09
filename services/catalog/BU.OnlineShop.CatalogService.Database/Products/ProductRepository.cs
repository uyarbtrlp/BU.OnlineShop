using BU.OnlineShop.CatalogService.EntityFrameworkCore;
using BU.OnlineShop.Shared.Extensions;
using BU.OnlineShop.Shared.Repository;
using Microsoft.EntityFrameworkCore;

namespace BU.OnlineShop.CatalogService.Products
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly CatalogServiceDbContext _dbContext;


        public ProductRepository(CatalogServiceDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<Product>> GetListAsync(
            Guid? categoryId = null,
            string name = null,
            string code = null,
            float? minPrice = null,
            float? maxPrice = null,
            int? minStockCount = null,
            int? maxStockCount = null
            )
        {
            var query = await GetListQueryAsync(
                categoryId,
                name,
                code,
                minPrice,
                maxPrice,
                minStockCount,
                maxStockCount);

            return await query.ToListAsync();
        }

        public async Task<long> GetCountAsync(
            Guid? categoryId = null,
            string name = null,
            string code = null,
            float? minPrice = null,
            float? maxPrice = null,
            int? minStockCount = null,
            int? maxStockCount = null)
        {
            var query = await GetListQueryAsync(
                categoryId,
                name,
                code,
                minPrice,
                maxPrice,
                minStockCount,
                maxStockCount);

            return await query.LongCountAsync();
        }

        private async Task<IQueryable<Product>> GetListQueryAsync(
            Guid? categoryId = null,
            string name = null, 
            string code = null,
            float? minPrice = null,
            float? maxPrice = null, 
            int? minStockCount = null, 
            int? maxStockCount = null)
        {
            var query = _dbContext.Set<Product>()
                .AsNoTracking()
                .WhereIf(categoryId.HasValue, e => e.CategoryId == categoryId)
                .WhereIf(!string.IsNullOrEmpty(name), e => e.Name.Contains(name))
                .WhereIf(!string.IsNullOrEmpty(code), e => e.Code.Contains(code))
                .WhereIf(minPrice.HasValue, e => e.Price >= minPrice)
                .WhereIf(maxPrice.HasValue, e => e.Price <= maxPrice)
                .WhereIf(minStockCount.HasValue, e => e.StockCount >= minStockCount)
                .WhereIf(maxStockCount.HasValue, e => e.StockCount <= maxStockCount);

            return query;
        }

        public async Task<Product> FindAsync(string code)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(x => x.Code == code);
        }

    }
}
