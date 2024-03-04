using BU.OnlineShop.CatalogService.Categories;
using BU.OnlineShop.CatalogService.Domain.Shared.Categories;
using BU.OnlineShop.CatalogService.Domain.Shared.Products;
using JetBrains.Annotations;

namespace BU.OnlineShop.CatalogService.Products
{
    public class ProductManager : IProductManager
    {
        protected IProductRepository ProductRepository { get; set; }
        protected ICategoryRepository CategoryRepository { get; set; }

        public ProductManager(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            CategoryRepository = categoryRepository;
            ProductRepository = productRepository;
        }

        public async Task<Product> CreateAsync(
            Guid categoryId,
            Guid imageId,
            string name,
            string code,
            float price,
            int stockCount)
        {
            await CheckCategory(categoryId);

            var existingProduct = await ProductRepository.FindAsync(code);

            if (existingProduct != null)
            {
                throw new ProductCodeExistException("Product code already exists.");
            }

            return new Product(
                  Guid.NewGuid(),
                  categoryId,
                  imageId,
                  code,
                  name,
                  price,
                  stockCount);
        }

        public async Task<Product> UpdateAsync(
            [NotNull] Product product,
            string name,
            string code,
            float price,
            int stockCount)
        {

            if (product.Code != code)
            {
                await CheckCode(code);
            }

            product.SetName(name);
            product.SetCode(code);
            product.SetPrice(price);
            product.SetStockCount(stockCount);

            return product;
        }

        private async Task CheckCode(string code)
        {
            var existingProduct = await ProductRepository.FindAsync(code);

            if (existingProduct != null)
            {
                throw new ProductCodeExistException("Product code already exists.");
            }
        }

        private async Task CheckCategory(Guid categoryId)
        {
            var category = await CategoryRepository.FindAsync(categoryId);

            if (category == null)
            {
                throw new CategoryNotFoundException("Category could not be found.");
            }
        }

    }
}
