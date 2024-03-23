using System.Text.Json;

using Azure.Messaging.ServiceBus;

namespace Deveel.Events {
	static class EventExtension {
		private static BinaryData? GetBinaryData(EventData data, JsonSerializerOptions? jsonOptions = null) {
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

		public static ServiceBusMessage AsMessage(this IEvent @event) {
			var body = GetBinaryData(@event.EventData);

			var message = new ServiceBusMessage {
				Body = body,
				MessageId = @event.EventId,
				ContentType = "application/json"
			};

			// TODO: extract the properies from the event data
			message.ApplicationProperties.Add("data.version", @event.DataVersion);
			message.ApplicationProperties.Add("event.type", @event.EventType);

			if (@event.Attributes != null) {
				foreach (var item in @event.Attributes) {
					message.ApplicationProperties.Add(item.Key, item.Value);
				}
			}

			return message;
		}
	}
}
