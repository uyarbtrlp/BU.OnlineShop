using BU.OnlineShop.OrderingService.EntityFrameworkCore;
using BU.OnlineShop.Shared.Repository;
using Microsoft.EntityFrameworkCore;

namespace BU.OnlineShop.OrderingService.Orders
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly OrderingServiceDbContext _dbContext;


        public OrderRepository(OrderingServiceDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Order>> GetListByUserId(Guid userId)
        {
            return await _dbContext.Orders.Include(x => x.OrderItems).Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
