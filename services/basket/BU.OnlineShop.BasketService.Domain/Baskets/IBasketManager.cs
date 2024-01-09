namespace BU.OnlineShop.BasketService.Baskets
{
    public interface IBasketManager
    {
        Task<Basket> CreateAsync(Guid userId);
    }
}
