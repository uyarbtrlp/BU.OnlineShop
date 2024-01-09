using BU.OnlineShop.Shared.Repository;

namespace BU.OnlineShop.OrderingService.Orders
{
    public interface IOrderRepository: IRepository<Order>
    {
        Task<IEnumerable<Order>> GetListByUserId(Guid userId);
    }
}
