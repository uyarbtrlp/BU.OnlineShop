using BU.OnlineShop.BasketService.API.Dtos.CatalogService;
using BU.OnlineShop.Shared.Extensions;

namespace BU.OnlineShop.BasketService.API.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductDto> GetAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/catalog-service/products/{id}");
            return await response.ReadContentAs<ProductDto>();
        }
    }
}
