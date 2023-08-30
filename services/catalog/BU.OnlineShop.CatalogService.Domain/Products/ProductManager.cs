using BU.OnlineShop.CatalogService.Categories;
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
            string name,
            string code,
            float price,
            int stockCount)
        {
            await CheckCategory(categoryId);

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

        public async Task<Product> UpdateAsync(
            [NotNull] Product product,
            Guid categoryId,
            string name,
            string code,
            float price,
            int stockCount)
        {
            if (product.CategoryId != categoryId)
            {
                await CheckCategory(categoryId);

            }

            if (product.Code != code)
            {
                await CheckCode(code);
            }

            product.SetCategoryId(categoryId);
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
                throw new Exception("Product code already exist.");
            }
        }

        private async Task CheckCategory(Guid categoryId)
        {
            var category = await CategoryRepository.FindAsync(categoryId);

            if (category == null)
            {
                throw new Exception("Category could not be found.");
            }
        }

    }
}
