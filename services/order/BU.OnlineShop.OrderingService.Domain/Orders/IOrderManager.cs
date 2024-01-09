namespace BU.OnlineShop.OrderingService.Orders
{
    public interface IOrderManager
    {
        Task<Order> CreateAsync(Guid userId, List<(Guid ProductId, string ProductName, string ProductCode, double ProductPrice, int ProductCount)> orderItems);
    }
}
