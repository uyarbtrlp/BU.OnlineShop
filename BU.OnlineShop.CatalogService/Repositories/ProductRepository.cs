using BU.OnlineShop.CatalogService.DbContexts;
using BU.OnlineShop.CatalogService.Entities;
using Microsoft.EntityFrameworkCore;

namespace BU.OnlineShop.CatalogService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogServiceDbContext _catalogServiceDbContext;


        public ProductRepository(CatalogServiceDbContext catalogServiceDbContext)
        {
            _catalogServiceDbContext = catalogServiceDbContext;
        }


        public async Task<Product> InsertAsync(Product product, bool autoSave = false)
        {
            var newProduct = await _catalogServiceDbContext.Products.AddAsync(product);

            if (autoSave)
            {
                await SaveChangesAsync();
            }

            return newProduct.Entity;
        }

        public async Task<IEnumerable<Product>> GetListAsync()
        {
            return await _catalogServiceDbContext.Products.ToListAsync();
        }

        public async Task<long> GetCountAsync()
        {
            return await _catalogServiceDbContext.Products.LongCountAsync();
        }
        public async Task<Product> GetAsync(Guid id)
        {
            var product = await _catalogServiceDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            return product;
        }
        public async Task<Product> FindAsync(Guid id)
        {
            return await _catalogServiceDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Product> FindAsync(string code)
        {
            return await _catalogServiceDbContext.Products.FirstOrDefaultAsync(x => x.Code == code);
        }
        public async Task<Product> UpdateAsync(Product product, bool autoSave = false)
        {
            _catalogServiceDbContext.Attach(product);

            var updatedProduct = _catalogServiceDbContext.Products.Update(product).Entity;

            if (autoSave)
            {
                await SaveChangesAsync();
            }

            return updatedProduct;
        }

        public async Task DeleteAsync(Product product, bool autoSave = false)
        {
            _catalogServiceDbContext.Products.Remove(product);

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
