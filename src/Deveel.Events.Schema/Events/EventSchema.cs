// 
//  Copyright 2023-2024 Antonello Provenzano
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

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
