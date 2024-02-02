﻿using BU.OnlineShop.Integration.MessageBus;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BU.OnlineShop.OrderingService.API.MessageBus
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _queue;
        private readonly ILogger<MessageBusSubscriber> _logger;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor, ILogger<MessageBusSubscriber> logger)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;

            InitializeRabbitMQ();
            _logger = logger;
        }

        private void InitializeRabbitMQ()
        {

            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Connections:Default:HostName"],
                UserName = _configuration["RabbitMQ:Connections:Default:UserName"],
                Password = _configuration["RabbitMQ:Connections:Default:Password"]
            };

            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: _configuration["RabbitMQ:EventBus:ExchangeName"], ExchangeType.Fanout);
            _queue = _channel.QueueDeclare(_configuration["RabbitMQ:EventBus:QueueName"], true, false, false, null).QueueName;
            _channel.QueueBind(queue: _queue, exchange: _configuration["RabbitMQ:EventBus:ExchangeName"], routingKey: "");
            _connection.ConnectionShutdown += RabbitMqConnectionShutDown;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (moduleHandle, ea) =>
            {
                var body = ea.Body;

                var message = Encoding.UTF8.GetString(body.ToArray());

                _eventProcessor.ProcessEventAsync(message, ea.RoutingKey);
            };

            _channel.BasicConsume(queue: _queue, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        private void RabbitMqConnectionShutDown(object? sender, ShutdownEventArgs e)
        {
            _logger.LogError($"Connection failed : {e.Exception.Message}");
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

            base.Dispose();
        }
    }
}
