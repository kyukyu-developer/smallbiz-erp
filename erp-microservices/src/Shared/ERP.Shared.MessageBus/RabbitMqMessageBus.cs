using System.Text;
using System.Text.Json;
using ERP.Shared.Contracts.Common;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ERP.Shared.MessageBus;

public class RabbitMqMessageBus : IMessageBus, IDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger<RabbitMqMessageBus> _logger;
    private const string ExchangeName = "erp_events";

    public RabbitMqMessageBus(string connectionString, ILogger<RabbitMqMessageBus> logger)
    {
        _logger = logger;

        var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        _channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic, durable: true).GetAwaiter().GetResult();
    }

    public void Publish<T>(T @event) where T : IntegrationEvent
    {
        var routingKey = typeof(T).Name;
        var message = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(message);

        var properties = new BasicProperties
        {
            Persistent = true,
            ContentType = "application/json"
        };

        _channel.BasicPublishAsync(ExchangeName, routingKey, false, properties, body).GetAwaiter().GetResult();
        _logger.LogInformation("Published event {EventType} with Id {EventId}", routingKey, @event.Id);
    }

    public void Subscribe<T>(Func<T, Task> handler) where T : IntegrationEvent
    {
        var routingKey = typeof(T).Name;
        var queueName = $"{routingKey}_queue";

        _channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false).GetAwaiter().GetResult();
        _channel.QueueBindAsync(queueName, ExchangeName, routingKey).GetAwaiter().GetResult();

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var @event = JsonSerializer.Deserialize<T>(message);

                if (@event != null)
                {
                    await handler(@event);
                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                    _logger.LogInformation("Processed event {EventType} with Id {EventId}", routingKey, @event.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing event {EventType}", routingKey);
                await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsumeAsync(queueName, autoAck: false, consumer).GetAwaiter().GetResult();
        _logger.LogInformation("Subscribed to event {EventType}", routingKey);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
