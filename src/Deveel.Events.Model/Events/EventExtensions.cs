using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Deveel.Events {
	public static class EventExtensions {
		public static IEvent WithEventId(this IEvent @event, string eventId)
			=> new EventWithId(@event, eventId);

		public static IEvent WithTimeStamp(this IEvent @event, DateTimeOffset timeStamp)
			=> new EventWithTimeStamp(@event, timeStamp);

		public static IEvent WithSource(this IEvent @event, string source)
			=> new EventWithSource(@event, source);

		public static IEvent With(this IEvent @event, IDictionary<string, object?> properties)
			=> new EventWithProperties(@event, properties);

		public static IEvent WithData(this IEvent @event, EventData eventData)
			=> new EventWithData(@event, eventData);

		public static IEvent WithBinaryData(this IEvent @event, byte[] data)
			=> @event.WithData(new EventData(data));

		public static IEvent WithBinaryData(this IEvent @event, string utf8Data)
			=> @event.WithBinaryData(Encoding.UTF8.GetBytes(utf8Data));

		public static IEvent WithJsonData(this IEvent @event, object? data, JsonSerializerOptions? jsonOptions = null)
			=> @event.WithData(new EventData(data, jsonOptions));

		class EventWithData : EventWrapper {
			private readonly EventData _eventData;

			public EventWithData(IEvent @event, EventData eventData)
				: base(@event) {
				_eventData = eventData;
			}

			public override EventData EventData => _eventData;
		}

		class EventWithProperties : EventWrapper {
			private readonly IDictionary<string, object?> _properties;

			public EventWithProperties(IEvent @event, IDictionary<string, object?> properties)
				: base(@event) {
				var merge = new Dictionary<string, object?>(@event.Attributes);
				foreach (var pair in properties) {
					merge[pair.Key] = pair.Value;
				}

				_properties = merge;
			}

			public override IDictionary<string, object?> Attributes => _properties;
		}

		class EventWithSource : EventWrapper {
			private readonly string _source;

			public EventWithSource(IEvent @event, string source)
				: base(@event) {
				_source = source;
			}

			public override string? Source => _source;
		}

		class EventWithTimeStamp : EventWrapper {
			private readonly DateTimeOffset _timeStamp;

			public EventWithTimeStamp(IEvent @event, DateTimeOffset timeStamp)
				: base(@event) {
				_timeStamp = timeStamp;
			}

			public override DateTimeOffset? TimeStamp => _timeStamp;
		}

		class EventWithId : EventWrapper {
			private readonly string _eventId;

			public EventWithId(IEvent @event, string eventId)
				: base(@event) {
				_eventId = eventId;
			}

			public override string? EventId => _eventId;
		}
	}
}
