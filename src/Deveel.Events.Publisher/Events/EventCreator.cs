// 
//  Copyright 2023-2024 Antonello Provenzano
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using CloudNative.CloudEvents;

using Microsoft.Extensions.Options;

using System.Reflection;
using System.Text.Json;

namespace Deveel.Events
{
    class EventCreator : IEventCreator
    {
        public EventCreator(IOptions<EventPublisherOptions>? publisherOptions = null)
        {
            PublisherOptions = publisherOptions?.Value ?? new EventPublisherOptions();
        }

        private EventPublisherOptions PublisherOptions { get; }

        public CloudEvent CreateEventFromData(Type dataType, object? data)
        {
            ArgumentNullException.ThrowIfNull(dataType, nameof(dataType));

            var eventAttribute = dataType.GetCustomAttribute<EventAttribute>();
            if (eventAttribute == null)
                throw new ArgumentException($"The type {dataType.FullName} is not an event type");

            var eventType = eventAttribute.EventType;
            var dataSchema = eventAttribute.DataSchema;
            var dataVersion = eventAttribute.DataVersion;

            Uri? schemaUri = null;
            if (dataSchema == null && String.IsNullOrWhiteSpace(dataVersion))
                throw new ArgumentException($"The event type {eventType} does not have a schema or version");

            if (dataSchema != null)
            {
                schemaUri = dataSchema;
            } else if (!String.IsNullOrWhiteSpace(dataVersion))
            {
                if (PublisherOptions.DataSchemaBaseUri == null)
                    throw new InvalidOperationException("The base URI for the data schema is not set");

                var schemaUriBuilder = new UriBuilder(PublisherOptions.DataSchemaBaseUri);
                schemaUriBuilder.Path = $"{schemaUriBuilder.Path}/{eventType}/{dataVersion}";
                schemaUri = schemaUriBuilder.Uri;
            }

            // TODO: discover an optional content type attribute and 
            //       use it to determine the content type of the data
            var @event = new CloudEvent
            {
                Type = eventType,
                DataSchema = schemaUri,
                DataContentType = "application/cloudevents+json",
                Data = JsonSerializer.Serialize(data, PublisherOptions.JsonSerializerOptions)
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
