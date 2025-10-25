using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
using ServerMonitoringAndNotificationSystem.Domin.Interfaces;
using ServerMonitoringAndNotificationSystem.Domin.configuration;

namespace RabbitMQService.Services
{
    public class RabbitMQProducer : IMessageProducer
    {
        private readonly RabbitMqConfig _config;

        public RabbitMQProducer(RabbitMqConfig config)
        {
            if (string.IsNullOrWhiteSpace(config.HostName))
                throw new ArgumentNullException(nameof(config.HostName), "Hostname cannot be null or empty");

            if (string.IsNullOrWhiteSpace(config.Exchange))
                throw new ArgumentNullException(nameof(config.Exchange), "Exchange cannot be null or empty");

            if (string.IsNullOrWhiteSpace(config.ExchangeType))
                throw new ArgumentNullException(nameof(config.ExchangeType), "Exchange type cannot be null or empty");

            _config = config;
        }

        public async Task ProduceAsync<T>(string routingKey, T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = _config.HostName,
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: _config.Exchange, type: _config.ExchangeType, durable: true);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: _config.Exchange,
                routingKey: routingKey,
                body: body);
        }
    }
}