using BU.OnlineShop.Shared.Repository;

namespace BU.OnlineShop.OrderingService.Orders
{
    public interface IOrderRepository: IRepository<Order>
    {
        Task<IEnumerable<Order>> GetListAsync(
            Guid? id = null,
            Guid? userId = null,
            OrderStatus? orderStatus = null);

        Task<long> GetCountAsync(
            Guid? id = null,
            Guid? userId = null,
            OrderStatus? orderStatus = null);
    }
}
