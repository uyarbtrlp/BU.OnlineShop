namespace BU.OnlineShop.OrderingService.Orders
{
    public interface IOrderManager
    {
        Task<Order> CreateAsync(Guid userId, List<OrderItem> orderItems);
    }
}
