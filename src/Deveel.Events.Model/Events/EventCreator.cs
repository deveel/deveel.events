using System.Reflection;

namespace Deveel.Events {
	static class EventCreator {
		public static Event CreateEventFromData(Type dataType, object? data) {
			ArgumentNullException.ThrowIfNull(dataType, nameof(dataType));

			var eventAttribute = dataType.GetCustomAttribute<EventAttribute>();
			if (eventAttribute == null)
				throw new ArgumentException($"The type {dataType.FullName} is not an event type");

			var eventType = eventAttribute.EventType;
			var dataVersion = eventAttribute.DataVersion;

			// TODO: discover an optional content type attribute and 
			//       use it to determine the content type of the data
			var @event = new Event(eventType, dataVersion) {
				EventData = new EventData(data, EventContentType.Object)
			};

			var eventAttrs = dataType.GetCustomAttributes<EventAttributesAttribute>();
			foreach (var attr in eventAttrs) {
				@event.Attributes[attr.AttributeName] = attr.Value;
			}

			return @event;
		}
	}
}
