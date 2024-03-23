namespace Deveel.Events {
	public interface IEventSchema {
		string EventType { get; }

		string Version { get; }

		string? Description { get; }

		string ContentType { get; }

		IEnumerable<IEventProperty> Properties { get; }
	}
}
