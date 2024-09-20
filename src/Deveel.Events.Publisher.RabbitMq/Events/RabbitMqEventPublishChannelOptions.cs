using CloudNative.CloudEvents;

using System.Text.Json;

namespace Deveel.Events
{
    public class RabbitMqEventPublishChannelOptions
    {
        public string? ExchangeName { get; set; }

        public string? RoutingKey { get; set; }

        public string QueueName { get; set; }

        public string ConnectionString { get; set; }

        public ContentMode ContentMode { get; set; } = ContentMode.Binary;

        public JsonSerializerOptions? JsonSerializerOptions { get; set; } = new JsonSerializerOptions();
    }
}
