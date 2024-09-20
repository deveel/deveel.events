using CloudNative.CloudEvents;
using CloudNative.CloudEvents.Http;
using CloudNative.CloudEvents.SystemTextJson;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

using RabbitMQ.Client;

using System.Text.Json;

namespace Deveel.Events
{
    public class RabbitMqEventPublishChannel : IEventPublishChannel, IDisposable
    {
        private readonly RabbitMqEventPublishChannelOptions _options;
        private readonly ILogger _logger;

        public RabbitMqEventPublishChannel(IOptions<RabbitMqEventPublishChannelOptions> options, 
            IConnection connection,
            ILogger<RabbitMqEventPublishChannel>? logger = null)
        {
            _connection = connection;
            _options = options.Value;
            _logger = logger ?? new NullLogger<RabbitMqEventPublishChannel>();

            _channel = _connection.CreateModel();
        }

        private readonly IConnection _connection;
        private readonly IModel _channel;

        public Task PublishAsync(CloudEvent @event, CancellationToken cancellationToken = default)
        {
            _logger.TracePublishingEvent(@event.Type);

            try
            {
                var exchangeName = GetExchangeName(@event);
                if (String.IsNullOrWhiteSpace(exchangeName))
                    throw new InvalidOperationException("The exchange name is not defined");

                var routingKey = GetRoutingKey(@event);
                if (String.IsNullOrWhiteSpace(routingKey))
                    throw new InvalidOperationException("The routing key is not defined");

                var formatter = new JsonEventFormatter(_options.JsonSerializerOptions, new JsonDocumentOptions());
                var json = formatter.ConvertToJsonElement(@event);
                var body = JsonSerializer.SerializeToUtf8Bytes(json, _options.JsonSerializerOptions);

                var props = _channel.CreateBasicProperties();
                props.ContentType = "application/cloudevents+json";
                props.ContentEncoding = "utf-8";

                _channel.BasicPublish(exchangeName, routingKey, props, body);
                return Task.CompletedTask;
            } catch (Exception ex)
            {
                _logger.LogErrorPublishingEvent(ex, @event.Type);
                throw;
            }
        }

        private string? GetRoutingKey(CloudEvent @event)
        {
            var exchangeNameAttr = @event.GetAttribute("amqproutingkey");
            return exchangeNameAttr != null && exchangeNameAttr.Type == CloudEventAttributeType.String
                ? ((string?)@event[exchangeNameAttr.Name]) ?? _options.RoutingKey
                : _options.RoutingKey;
        }

        private string? GetExchangeName(CloudEvent @event)
        {
            var exchangeName = @event.GetAttribute("amqpexchange");
            return exchangeName != null && exchangeName.Type == CloudEventAttributeType.String
                ? ((string?)@event[exchangeName.Name]) ?? _options.ExchangeName
                : _options.ExchangeName;
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}
