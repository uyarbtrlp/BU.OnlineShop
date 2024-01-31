namespace BU.OnlineShop.CatalogService.API.MessageBus
{
    public interface IEventProcessor
    {
        Task ProcessEventAsync(string message, string routingKey);
    }
}
