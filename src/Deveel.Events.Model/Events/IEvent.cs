namespace Deveel.Events {
	public interface IEvent {
		string? EventId { get; }

		string EventType { get; }

		DateTimeOffset? TimeStamp { get; }

		string DataVersion { get; }

		string? Source { get; }

		EventData EventData { get; }

		IDictionary<string, object?> Attributes { get; }
	}
}
