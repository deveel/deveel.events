//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using CloudNative.CloudEvents;
using CloudNative.CloudEvents.SystemTextJson;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

using RabbitMQ.Client;

using System.Text.Json;

namespace Deveel.Events
{
    /// <summary>
    /// The implementation of the <see cref="IEventPublishChannel"/> that
    /// is used to publish events to a RabbitMQ exchange.
    /// </summary>
    public sealed class RabbitMqEventPublishChannel : IEventPublishChannel, IDisposable
    {
        private readonly RabbitMqEventPublishChannelOptions _options;
        private readonly IRabbitMqMessageFactory _messageFactory;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructs the channel with the options and connection to the RabbitMQ server.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="connection"></param>
        /// <param name="messageFactory"></param>
        /// <param name="logger"></param>
        public RabbitMqEventPublishChannel(IOptions<RabbitMqEventPublishChannelOptions> options, 
            IConnection connection,
            IRabbitMqMessageFactory messageFactory,
            ILogger<RabbitMqEventPublishChannel>? logger = null)
        {
            _connection = connection;
            _messageFactory = messageFactory;
            _options = options.Value;
            _logger = logger ?? new NullLogger<RabbitMqEventPublishChannel>();

            _channel = _connection.CreateModel();
        }

        private readonly IConnection _connection;
        private readonly IModel _channel;

        /// <inheritdoc/>
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

                var message = _messageFactory.CreateMessage(@event);

                var props = _channel.CreateBasicProperties();
                props.ContentType = message.ContentType;
                props.ContentEncoding = message.ContentEncoding;
                props.Type = @event.Type;

                _channel.BasicPublish(exchangeName, routingKey, props, message.Body);

                return Task.CompletedTask;
            } catch (Exception ex)
            {
                _logger.LogErrorPublishingEvent(ex, @event.Type);
                throw;
            }
        }

        private string? GetRoutingKey(CloudEvent @event)
        {
            var exchangeNameAttr = @event.GetAttribute(AmqpCloudEventAttributes.AmqpRoutingKeyAttribute);
            return exchangeNameAttr != null && exchangeNameAttr.Type == CloudEventAttributeType.String
                ? ((string?)@event[exchangeNameAttr.Name]) ?? _options.RoutingKey
                : _options.RoutingKey;
        }

        private string? GetExchangeName(CloudEvent @event)
        {
            var exchangeName = @event.GetAttribute(AmqpCloudEventAttributes.AmqpExchangeNameAttribute);
            return exchangeName != null && exchangeName.Type == CloudEventAttributeType.String
                ? ((string?)@event[exchangeName.Name]) ?? _options.ExchangeName
                : _options.ExchangeName;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}
