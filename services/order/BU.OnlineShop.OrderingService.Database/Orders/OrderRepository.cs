using BU.OnlineShop.OrderingService.EntityFrameworkCore;
using BU.OnlineShop.Shared.Extensions;
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

        public async Task<IEnumerable<Order>> GetListAsync(
            Guid? id = null,
            Guid? userId = null,
            OrderStatus? orderStatus = null
         )
        {
            var query = await GetListQueryAsync(
                id,
                userId,
                orderStatus);

            return await query.ToListAsync();
        }

        public async Task<long> GetCountAsync(
            Guid? id = null,
            Guid? userId = null,
            OrderStatus? orderStatus = null)
        {
            var query = await GetListQueryAsync(
                id,
                userId,
                orderStatus);

            return await query.LongCountAsync();
        }

        private async Task<IQueryable<Order>> GetListQueryAsync(
            Guid? id = null,
            Guid? userId = null,
            OrderStatus? orderStatus = null)
        {
            var query = _dbContext.Set<Order>().Include(x => x.OrderItems)
                .AsNoTracking()
                .WhereIf(id.HasValue, e => e.Id == id)
                .WhereIf(userId.HasValue, e => e.UserId == userId)
                .WhereIf(orderStatus.HasValue, e => e.OrderStatus == orderStatus);

            return query;
        }
    }
}
