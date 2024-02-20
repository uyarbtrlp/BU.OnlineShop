namespace BU.OnlineShop.OrderingService.API.MessageBus
{
    public interface IEventProcessor
    {
        Task ProcessEventAsync(string message, string routingKey);
    }
}
