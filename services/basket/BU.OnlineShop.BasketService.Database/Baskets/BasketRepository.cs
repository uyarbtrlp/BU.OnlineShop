using BU.OnlineShop.BasketService.EntityFrameworkCore;
using BU.OnlineShop.Shared.Exceptions;
using BU.OnlineShop.Shared.Repository;
using Microsoft.EntityFrameworkCore;

namespace BU.OnlineShop.BasketService.Baskets
{
    public class BasketRepository : Repository<Basket>, IBasketRepository
    {
        private readonly BasketServiceDbContext _dbContext;

        public BasketRepository(BasketServiceDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Basket> GetByUserIdAsync(Guid userIid)
        {
            var basket = await _dbContext.Baskets
                .Include(x=>x.BasketLines)
                .FirstOrDefaultAsync(x => x.UserId == userIid);

            if (basket == null)
            {
                throw new EntityNotFoundException(userIid, nameof(Basket));
            }

            return basket;
        }

        public async Task<Basket> FindByUserIdAsync(Guid userId)
        {
            return await _dbContext.Baskets.Include(x=> x.BasketLines).FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<bool> ExistAsync(Guid id)
        {
           return await _dbContext.Baskets.AnyAsync(x => x.UserId == id || x.Id == id);
        }
    }
}
