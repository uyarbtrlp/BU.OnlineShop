using BU.OnlineShop.Integration.Messages;
using Microsoft.Extensions.Configuration;
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

        public RabbitMqMessageBus(IConfiguration configuration)
        {
            _configuration = configuration;

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
               _channel.ExchangeDeclare(exchange: _configuration["RabbitMQ:EventBus:ExchangeName"], type: ExchangeType.Topic);
               _channel.QueueDeclare(_configuration["RabbitMQ:EventBus:QueueName"], true, false, false, null);
               _connection.ConnectionShutdown += RabbitMqConnectionShutDown;
            }

            catch(Exception ex)
            {
                //TODO: Logging
                Console.WriteLine($"Connection failed : {ex.Message}");
            }
        }


        public async Task PublishMessageAsync(BaseEto message, string queue, string routingKey)
        {

            if (_connection.IsOpen)
            {
                _channel.QueueBind(queue: queue, exchange: _configuration["RabbitMQ:EventBus:ExchangeName"], routingKey: routingKey);
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
            //TODO: Logging
            Console.WriteLine($"Connection lost : {e.Exception}");
        }
    }
}
