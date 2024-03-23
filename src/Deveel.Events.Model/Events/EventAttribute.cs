using System;

namespace Deveel.Events {
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class EventAttribute : Attribute {
		public EventAttribute(string eventType, string dataVersion) {
			ArgumentNullException.ThrowIfNull(eventType, nameof(eventType));
			ArgumentNullException.ThrowIfNull(dataVersion, nameof(dataVersion));

			EventType = eventType;
			DataVersion = dataVersion;
		}

		public string EventType { get; }

		public string DataVersion { get; }

		public string? Description { get; set; }
	}
}
