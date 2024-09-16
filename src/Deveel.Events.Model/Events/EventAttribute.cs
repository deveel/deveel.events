using System;

namespace Deveel.Events {
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class EventAttribute : Attribute {
		public EventAttribute(string eventType, string dataSchemaOrVersion) {
			ArgumentNullException.ThrowIfNull(eventType, nameof(eventType));
			ArgumentNullException.ThrowIfNull(dataSchemaOrVersion, nameof(dataSchemaOrVersion));

			EventType = eventType;

			if (Uri.TryCreate(dataSchemaOrVersion, UriKind.Absolute, out var uri))
			{
                DataSchema = uri;
            } else
			{
                DataVersion = dataSchemaOrVersion;
            }
		}

		public string EventType { get; }

		public Uri? DataSchema { get; }

		public string? DataVersion { get; set; }

		public string? Description { get; set; }
	}
}
