
namespace Deveel.Events {
	public class Event : IEvent {
		public Event(string eventType, string dataVersion) {
			ArgumentNullException.ThrowIfNull(eventType, nameof(eventType));
			ArgumentNullException.ThrowIfNull(dataVersion, nameof(dataVersion));

			EventType = eventType;
			DataVersion = dataVersion;
			Attributes = new Dictionary<string, object?>();
			EventData = EventData.Empty;
		}

		public string? EventId { get; set; }

		public string EventType { get; }

		public DateTimeOffset? TimeStamp { get; set; }

		public string DataVersion { get; }

		public string? Source { get; set; }

		public EventData EventData { get; set; }

		public IDictionary<string, object?> Attributes { get; }

		public static Event FromData(Type dataType, object? data)
			=> EventCreator.CreateEventFromData(dataType, data);

		public static Event FromData<T>(T? data)
			=> EventCreator.CreateEventFromData(typeof(T), data);
	}
}
