//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

namespace Deveel.Events {
	public class EventSchema : IEventSchema, IVersionedElement {
		public EventSchema(string eventType, string version, string contentType) {
			ArgumentNullException.ThrowIfNull(eventType, nameof(eventType));
			ArgumentNullException.ThrowIfNull(version, nameof(version));
			ArgumentNullException.ThrowIfNull(contentType, nameof(contentType));

			if (!Version.TryParse(version, out var schemaVersion))
				throw new ArgumentException("The version string is not valid", nameof(version));

			EventType = eventType;
			Version = schemaVersion;

			Properties = new EventPropertyCollection(this);
			ContentType = contentType;
		}

		public string EventType { get; }

		string IEventSchema.Version => Version.ToString();

		public Version Version { get; }

		public string? Description { get; set; }

		public string ContentType { get; }

		public EventPropertyCollection Properties { get; }

		IEnumerable<IEventProperty> IEventSchema.Properties => Properties;

		public static EventSchema FromDataType(Type dataType)
			=> EventSchemaCreator.FromEventData(dataType);

		public static EventSchema FromDataType<TData>()
			where TData : class
			=> FromDataType(typeof(TData));
	}
}
