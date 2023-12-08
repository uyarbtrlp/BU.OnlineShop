using BU.OnlineShop.BasketService.API.Dtos.CatalogService;

namespace BU.OnlineShop.BasketService.API.Services
{
    public interface ICatalogService
    {
        Task<ProductDto> GetAsync(Guid id);
    }
}
