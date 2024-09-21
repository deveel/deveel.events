//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using CloudNative.CloudEvents;

using System.Text.Json;

namespace Deveel.Events
{
    /// <summary>
    /// Configuration options for a <see cref="RabbitMqEventPublishChannel"/>.
    /// </summary>
    public class RabbitMqEventPublishChannelOptions
    {
        /// <summary>
        /// The default exchange name to publish the events to,
        /// when not explicitly defined in the event.
        /// </summary>
        public string? ExchangeName { get; set; }

        /// <summary>
        /// The default routing key to use when publishing the events,
        /// when not explicitly defined in the event.
        /// </summary>
        public string? RoutingKey { get; set; }

        /// <summary>
        /// The name of the queue to bind the exchange to.
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// The connection string to the RabbitMQ server.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// A set of options to configure the JSON serialization
        /// when the message format is set to <see cref="RabbitMqMessageFormat.Json"/>.
        /// </summary>
        public JsonSerializerOptions? JsonSerializerOptions { get; set; } = new JsonSerializerOptions();

        /// <summary>
        /// The format of the message to be published to
        /// the RabbitMQ queue.
        /// </summary>
        public RabbitMqMessageFormat MessageFormat { get; set; } = RabbitMqMessageFormat.Json;

        /// <summary>
        /// The type of content to be published to the RabbitMQ queue.
        /// </summary>
        public RabbitMqMessageContent MessageContent { get; set; } = RabbitMqMessageContent.CloudEvent;
    }
}
