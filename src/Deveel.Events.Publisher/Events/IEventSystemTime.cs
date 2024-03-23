namespace Deveel.Events {
	public interface IEventSystemTime {
		DateTimeOffset UtcNow { get; }
	}
}
