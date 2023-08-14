using BU.OnlineShop.CatalogService.Entities;
using BU.OnlineShop.CatalogService.Repositories;

namespace BU.OnlineShop.CatalogService.Services
{
    //TODO: layerlama ve generic repo, unit of work. daha sonra category ile ilgili endpointler
    public class ProductManager : IProductManager
    {
        protected IProductRepository ProductRepository { get; set; }
        protected ICategoryRepository CategoryRepository { get; set; }

        public ProductManager(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            CategoryRepository = categoryRepository;
            ProductRepository = productRepository;
        }

        public async Task<Product> CreateProductAsync(
            Guid categoryId,
            string name,
            string code,
            float price,
            int stockCount)
        {
            //await CheckCategory(categoryId);

            var existingProduct = await ProductRepository.FindAsync(code);

            if (existingProduct != null)
            {
                throw new Exception("Product code already exist.");
            }

            return new Product(
                  Guid.NewGuid(),
                  categoryId,
                  code,
                  name,
                  price,
                  stockCount);
        }

        public async Task CheckCategory(Guid categoryId)
        {
            var category = await CategoryRepository.FindAsync(categoryId);

            if (category == null)
            {
                throw new Exception("Category can not be null.");
            }
        }
    }
}
