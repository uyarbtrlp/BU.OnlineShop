using BU.OnlineShop.Shared.Repository;

namespace BU.OnlineShop.BasketService.Domain.Baskets
{
    public interface IBasketRepository : IRepository<Basket>
    {
        Task<Basket> FindByUserIdAsync(Guid userId);

        Task<Basket> GetByUserIdAsync(Guid userId);

        Task<bool> ExistAsync(Guid id);
    }
}
