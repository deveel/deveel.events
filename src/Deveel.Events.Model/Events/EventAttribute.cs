using System;

namespace Deveel.Events {
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class EventAttribute : Attribute {
		public EventAttribute(string eventType, string dataSchema) {
			ArgumentNullException.ThrowIfNull(eventType, nameof(eventType));
			ArgumentNullException.ThrowIfNull(dataSchema, nameof(dataSchema));

			EventType = eventType;
			DataSchema = dataSchema;
		}

		public string EventType { get; }

		public string DataSchema { get; }

		public string? Description { get; set; }
	}
}
