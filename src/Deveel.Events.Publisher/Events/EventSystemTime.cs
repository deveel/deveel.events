namespace Deveel.Events {
	public sealed class EventSystemTime : IEventSystemTime {
		public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

		public static readonly EventSystemTime Instance = new EventSystemTime();
	}
}
