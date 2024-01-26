namespace BU.OnlineShop.OrderingService.Orders
{
    public class OrderManager : IOrderManager
    {
        public async Task<Order> CreateAsync(Guid userId, List<OrderItem> orderItems)
        {
            Order order = new Order(
                id: new Guid(),
                userId: userId,
                orderStatus: OrderStatus.Placed
                );

            foreach ( var orderItem in orderItems )
            {
                order.AddOrderItem(
                    id: new Guid(),
                    productId: orderItem.ProductId,
                    productName: orderItem.ProductName,
                    productCode: orderItem.ProductCode,
                    productCount: orderItem.ProductCount,
                    productPrice: orderItem.ProductPrice);
            }

            return await Task.FromResult(order);
        }
    }
}
