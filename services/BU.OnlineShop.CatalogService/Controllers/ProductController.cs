using AutoMapper;
using BU.OnlineShop.CatalogService.Entities;
using BU.OnlineShop.CatalogService.Models.Products;
using BU.OnlineShop.CatalogService.Repositories;
using BU.OnlineShop.CatalogService.Services;
using Microsoft.AspNetCore.Mvc;

namespace BU.OnlineShop.CatalogService.Controllers
{
    [Route("api/catalog-service/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductManager _productManager;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IProductManager productManager, IMapper mapper)
        {
            _productRepository = productRepository;
            _productManager = productManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ProductDto> CreateAsync(CreateProductInput input)
        {

            var product = await _productManager.CreateProductAsync(
                    categoryId: input.CategoryId,
                    name: input.Name,
                    code: input.Code,
                    price: input.Price,
                    stockCount: input.StockCount
                );

            await _productRepository.InsertAsync(
                  product, true);


          return _mapper.Map<ProductDto>(product);
        }

        [HttpGet]
        public async Task<List<ProductDto>> GetListAsync()
        {
            var products = await _productRepository.GetListAsync();

            return _mapper.Map<List<Product>,List<ProductDto>>(products.ToList());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ProductDto> UpdateAsync(Guid id, UpdateProductInput input)
        {
            var product = await _productRepository.FindAsync(id);

            product.SetName(input.Name);
            product.SetPrice(input.Price);
            product.SetPrice(input.StockCount);

            product = await _productRepository.UpdateAsync(product, true);

            return _mapper.Map<ProductDto>(product);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            var product = await _productRepository.FindAsync(id);

            await _productRepository.DeleteAsync(product, true);
        }
    }
}
