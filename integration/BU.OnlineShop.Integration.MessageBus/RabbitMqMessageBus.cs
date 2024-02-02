using BU.OnlineShop.Integration.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace BU.OnlineShop.Integration.MessageBus
{
    public class RabbitMqMessageBus : IMessageBus
    {
        private readonly IConfiguration _configuration;

        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<RabbitMqMessageBus> _logger;

        public RabbitMqMessageBus(IConfiguration configuration, ILogger<RabbitMqMessageBus> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Connections:Default:HostName"],
                UserName = _configuration["RabbitMQ:Connections:Default:UserName"],
                Password = _configuration["RabbitMQ:Connections:Default:Password"]
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: _configuration["RabbitMQ:EventBus:ExchangeName"], type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMqConnectionShutDown;
            }

            catch (Exception ex)
            {
                _logger.LogError($"Connection failed : {ex.Message}");
            }
        }


        public async Task PublishMessageAsync(BaseEto message,string routingKey)
        {

            if (_connection.IsOpen)
            {
                //Serialize the message
                var json = JsonSerializer.Serialize<object>(message);
                var body = Encoding.UTF8.GetBytes(json);

                _channel.BasicPublish(exchange: _configuration["RabbitMQ:EventBus:ExchangeName"], routingKey: routingKey, basicProperties: null, body: body);
            }


            await Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void RabbitMqConnectionShutDown(object? sender, ShutdownEventArgs e)
        {
            _logger.LogError($"Connection failed : {e.Exception.Message}");
        }
    }
}
