namespace BU.OnlineShop.OrderingService.API.Dtos.Orders
{
    public class OrderItemDto
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductCode { get; set; }

        public double ProductPrice { get; set; }

        public int ProductCount { get; set; }
    }
}
