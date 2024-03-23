

namespace Deveel.Events {
	class EventWrapper : IEvent {
		private readonly IEvent _event;

		public EventWrapper(IEvent @event) {
			_event = @event;
		}

		public virtual string? EventId => _event.EventId;

		public virtual string EventType => _event.EventType;

		public virtual DateTimeOffset? TimeStamp => _event.TimeStamp;

		public virtual string DataVersion => _event.DataVersion;

		public virtual string? Source => _event.Source;

		public virtual EventData EventData => _event.EventData;

		public virtual IDictionary<string, object?> Attributes => _event.Attributes;
	}
}
