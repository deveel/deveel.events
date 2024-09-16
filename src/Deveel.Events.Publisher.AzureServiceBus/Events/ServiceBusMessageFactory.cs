using System.Text;
using System.Text.Json;

using Azure.Messaging.ServiceBus;

using CloudNative.CloudEvents;

namespace Deveel.Events {
	public class ServiceBusMessageFactory {
		protected virtual BinaryData? GetBinaryData(string? contentType, object? data) {
			if (contentType == null)
				return null;

			var typePart = contentType.Substring(contentType.IndexOf('/') + 1);
			if (typePart.Length < 2)
                throw new ArgumentException("The content type is not valid");

			BinaryData binaryData;
			if (data == null) {
				return null;
			} else if (typePart.EndsWith("binary") ||
				typePart == "octet-stream") {
				if (data is byte[] bytes)
				{
					binaryData = new BinaryData(bytes);
				} else if (data is string s)
				{
                    var binary = Convert.FromBase64String(s);
                    binaryData = new BinaryData(binary);
				} else
				{
					throw new ArgumentException("The data is not a valid binary format");
				}
			} else if (typePart.EndsWith("json")) {
				if (data is string s)
				{
					binaryData = new BinaryData(Encoding.UTF8.GetBytes(s));
				} else
				{
					// TODO: json options ...
                    var json = JsonSerializer.Serialize(data);
                    binaryData = new BinaryData(Encoding.UTF8.GetBytes(json));
                }
			} else {
				throw new InvalidOperationException("The content type of the event data is not supported");
			}

			return binaryData;
		}

		protected virtual string? GetSubject(CloudEvent @event) => @event.Subject;

		// TODO: get the correlation id from the event
		//       from a configured attribute
		protected virtual string GetCorrelationId(CloudEvent @event) => "";

		protected virtual void AddProperties(IDictionary<string, object> properties, CloudEvent @event)
		{
			if (@event.DataSchema != null)
                properties.Add(ServiceBusMessageProperties.DataVersion, @event.DataSchema.ToString());

			properties.Add(ServiceBusMessageProperties.EventType, @event.Type!);
			properties.Add(ServiceBusMessageProperties.TimeStamp, @event.Time!);

			foreach (var item in @event.GetPopulatedAttributes())
			{
				properties.Add(item.Key.Name, item.Value);
			}
		}

		public ServiceBusMessage CreateMessage(CloudEvent @event)
		{
			var body = GetBinaryData(@event.DataContentType, @event.Data);

			var message = new ServiceBusMessage
			{
				Body = body,
				MessageId = @event.Id,
				ContentType = @event.DataContentType,
				Subject = GetSubject(@event),
				CorrelationId = GetCorrelationId(@event)
				// TODO: extract the partition key from the event
			};

			AddProperties(message.ApplicationProperties, @event);

			return message;
		}
	}
}
