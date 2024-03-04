using AutoMapper;
using BU.OnlineShop.CatalogService.API.Dtos.Products;
using BU.OnlineShop.CatalogService.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BU.OnlineShop.CatalogService.Controllers
{
    [Route("api/catalog-service/products")]
    [ApiController]
    [Authorize(Roles = "Admin")]
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

            var product = await _productManager.CreateAsync(
                    categoryId: input.CategoryId,
                    imageId: input.ImageId,
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
        [Route("{id}")]
        [AllowAnonymous]
        public async Task<ProductDto> GetAsync(Guid id)
        {
            var test = HttpContext.User.Claims;
            var product = await _productRepository.GetAsync(id);

            return _mapper.Map<Product, ProductDto>(product);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<List<ProductDto>> GetListAsync([FromQuery] GetProductsInput input)
        {
            var products = await _productRepository.GetListAsync(
                    categoryId: input.CategoryId,
                    name: input.Name,
                    code: input.Code,
                    minPrice: input.MinPrice,
                    maxPrice: input.MaxPrice,
                    minStockCount: input.MinStockCount,
                    maxStockCount: input.MaxStockCount
                );

            return _mapper.Map<List<Product>,List<ProductDto>>(products.ToList());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ProductDto> UpdateAsync(Guid id, UpdateProductInput input)
        {
            var product = await _productRepository.GetAsync(id);

            product = await _productManager.UpdateAsync(
                product,
                name: input.Name,
                code: input.Code,
                price: input.Price,
                stockCount: input.StockCount);

             await _productRepository.UpdateAsync(product, true);

            return _mapper.Map<ProductDto>(product);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            var product = await _productRepository.GetAsync(id);

            await _productRepository.DeleteAsync(product, true);
        }
    }
}
