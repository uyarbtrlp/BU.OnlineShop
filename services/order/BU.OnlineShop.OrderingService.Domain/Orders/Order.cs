using BU.OnlineShop.Shared.Entities;

namespace BU.OnlineShop.OrderingService.Orders
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; protected set; }

        public OrderStatus OrderStatus { get; protected set; }

        public List<OrderItem> OrderItems { get; protected set; }


        protected Order()
        {

        }

        public Order(
            Guid id,
            Guid userId,
            OrderStatus orderStatus = OrderStatus.Placed)
        {
            Id = id;
            UserId = userId;
            SetOrderStatus(orderStatus);

            OrderItems = new List<OrderItem>();
        }

        public void SetOrderStatus(OrderStatus orderStatus)
        {
            OrderStatus = orderStatus;
        }

        public Order AddOrderItem(Guid id, Guid productId, string productName, string productCode, double productPrice, int productCount)
        {
            var orderItem = new OrderItem(
                id: id,
                productId: productId,
                productName: productName,
                productCode: productCode,
                productPrice: productPrice,
                productCount: productCount
                );

            OrderItems.Add( orderItem );

            return this;
        }
    }
}
