using BU.OnlineShop.Integration.Messages;

namespace BU.OnlineShop.Integration.MessageBus
{
    public interface IMessageBus
    {
        Task PublishMessageAsync(BaseEto message, string queue, string routingKey);
    }
}
