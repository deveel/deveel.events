using CloudNative.CloudEvents;

using System.Reflection;
using System.Text.Json;

namespace Deveel.Events
{
    public sealed class EventCreator : IEventCreator
    {
        public CloudEvent CreateEventFromData(Type dataType, object? data, JsonSerializerOptions? jsonOptions = null)
        {
            ArgumentNullException.ThrowIfNull(dataType, nameof(dataType));

            var eventAttribute = dataType.GetCustomAttribute<EventAttribute>();
            if (eventAttribute == null)
                throw new ArgumentException($"The type {dataType.FullName} is not an event type");

            var eventType = eventAttribute.EventType;
            var dataSchema = eventAttribute.DataSchema;

            Uri? schemaUri = null;
            if (dataSchema != null && !Uri.TryCreate(dataSchema, UriKind.Absolute, out schemaUri))
                throw new ArgumentException("The data schema is not a valid URI");

            // TODO: discover an optional content type attribute and 
            //       use it to determine the content type of the data
            var @event = new CloudEvent
            {
                Type = eventType,
                DataSchema = schemaUri,
                DataContentType = "application/cloudevents+json",
                Data = JsonSerializer.Serialize(data, jsonOptions)
            };

            var eventAttrs = dataType.GetCustomAttributes<EventAttributesAttribute>();
            foreach (var attr in eventAttrs)
            {
                @event[attr.AttributeName] = attr.Value;
            }

            return @event;
        }
    }
}
