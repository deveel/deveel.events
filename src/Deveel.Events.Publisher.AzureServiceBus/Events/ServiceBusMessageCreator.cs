using System.Text.Json;

using Azure.Messaging.ServiceBus;

namespace Deveel.Events {
	public class ServiceBusMessageCreator {
		protected virtual BinaryData? GetBinaryData(EventData data, JsonSerializerOptions? jsonOptions = null) {
			BinaryData binaryData;
			if (data == EventData.Empty ||
				data.Content == null) {
				return null;
			} else if (data.ContentType == EventContentType.Binary) {
				binaryData = new BinaryData(data.AsBinary());
			} else if (data.ContentType == EventContentType.Json) {
				binaryData = new BinaryData(data.AsJson(jsonOptions));
			} else if (data.ContentType == EventContentType.Object) {
				binaryData = new BinaryData(data.AsJson(jsonOptions));
			} else {
				throw new InvalidOperationException("The content type of the event data is not supported");
			}

			return binaryData;
		}

		protected virtual string GetContentType(EventData data) {
			if (data.ContentType == EventContentType.Binary) {
				return "application/octet-stream";
			} else if (data.ContentType == EventContentType.Json) {
				return "application/json";
			} else if (data.ContentType == EventContentType.Object) {
				return "application/json";
			} else {
				throw new InvalidOperationException("The content type of the event data is not supported");
			}
		}

		protected virtual string GetSubject(IEvent @event) => @event.Source ?? "";

		// TODO: get the correlation id from the event
		//       from a configured attribute
		protected virtual string GetCorrelationId(IEvent @event) => "";

		protected virtual void AddProperties(IDictionary<string, object> properties, IEvent @event) {
			properties.Add(ServiceBusMessageProperties.DataVersion, @event.DataVersion);
			properties.Add(ServiceBusMessageProperties.EventType, @event.EventType);
			properties.Add(ServiceBusMessageProperties.TimeStamp, @event.TimeStamp!);

			if (@event.Attributes != null) {
				foreach (var item in @event.Attributes) {
					properties.Add(item.Key, item.Value);
				}
			}
		}

		public ServiceBusMessage CreateMessage(IEvent @event) {
			var body = GetBinaryData(@event.EventData);

			var message = new ServiceBusMessage {
				Body = body,
				MessageId = @event.EventId,
				ContentType = GetContentType(@event.EventData),
				Subject = GetSubject(@event),
				CorrelationId = GetCorrelationId(@event)
				// TODO: extract the partition key from the event
			};

			AddProperties(message.ApplicationProperties, @event);

			if (@event.Attributes != null) {
				foreach (var item in @event.Attributes) {
					message.ApplicationProperties.Add(item.Key, item.Value);
				}
			}

			return message;
		}
	}
}
