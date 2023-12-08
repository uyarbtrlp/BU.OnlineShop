namespace BU.OnlineShop.BasketService.Domain.Baskets
{
    public interface IBasketManager
    {
        Task<Basket> CreateAsync(Guid userId);
    }
}
