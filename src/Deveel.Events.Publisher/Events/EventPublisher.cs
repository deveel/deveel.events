using CloudNative.CloudEvents;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Deveel.Events {
	public class EventPublisher {
		private readonly IEnumerable<IEventPublishChannel> _channels;
		private readonly IEventSystemTime _systemTime;
		private readonly IEventIdGenerator _idGenerator;
		private readonly ILogger _logger;

		public EventPublisher(
			IOptions<EventPublisherOptions> options, 
			IEnumerable<IEventPublishChannel> channels,
			IEventCreator? eventCreator = null,
			IEventIdGenerator? idGenerator = null,
			IEventSystemTime? systemTime = null,
			ILogger<EventPublisher>? logger = null) {
			_channels = channels;
            EventCreator = eventCreator;
            _idGenerator = idGenerator ?? EventGuidGenerator.Default;
			_systemTime = systemTime ?? EventSystemTime.Instance;
			_logger = logger ?? NullLogger<EventPublisher>.Instance;
			PublisherOptions = options.Value;
		}

		protected EventPublisherOptions PublisherOptions { get; }

        protected IEventCreator? EventCreator { get; }

        protected virtual Task PublishEventAsync(IEventPublishChannel channel, CloudEvent @event, CancellationToken cancellationToken) {
			return channel.PublishAsync(@event, cancellationToken);
		}

		protected virtual CloudEvent SetTimeStamp(CloudEvent @event) {
			if (@event.Time == null)
				@event.Time = _systemTime.UtcNow;

			return @event;
		}

		protected virtual CloudEvent SetSource(CloudEvent @event) {
			if (@event.Source == null && PublisherOptions.Source != null)
				@event.Source = PublisherOptions.Source;

			return @event;
		}

		protected virtual CloudEvent SetEventId(CloudEvent @event) {
			if (@event.Id == null)
				@event.Id = _idGenerator.GenerateId();

			return @event;
		}

		protected virtual CloudEvent SetAttributes(CloudEvent @event) {
			if (PublisherOptions.Attributes != null)
			{
				foreach (var attribute in PublisherOptions.Attributes)
				{
					var attr = CloudEventAttribute.CreateExtension(attribute.Key, GetAttributeType(attribute.Value));
					@event[attr] = attribute.Value;
                }
			}

			return @event;
		}

        private CloudEventAttributeType GetAttributeType(object? value)
        {
			return value switch
			{
				bool => CloudEventAttributeType.Boolean,
				byte[] _ => CloudEventAttributeType.Binary,
				string _ => CloudEventAttributeType.String,
				int _ => CloudEventAttributeType.Integer,
				long _ => CloudEventAttributeType.Integer,
				Uri _ => CloudEventAttributeType.Uri,
				DateTimeOffset _ => CloudEventAttributeType.Timestamp,
				DateTime _ => CloudEventAttributeType.Timestamp,
				_ => CloudEventAttributeType.String
			};

            throw new NotImplementedException();
        }

        public async Task PublishEventAsync(CloudEvent @event, CancellationToken cancellationToken = default) {
			// TODO: validate the event before publishing

			var eventToPublish = @event;
			@event = SetEventId(@event);
			@event = SetTimeStamp(@event);
			@event = SetSource(@event);
			@event = SetAttributes(@event);

			foreach (var channel in _channels) {
				_logger.TraceEventPublishing(@event.Type!, channel.GetType());

				try {
					await PublishEventAsync(channel, @event, cancellationToken);

					_logger.TraceEventPublished(@event.Type!, channel.GetType());
				} catch (Exception ex) {
					_logger.LogEventPublishError(ex, @event.Type!, channel.GetType());
					
					if (PublisherOptions.ThrowOnErrors) {
						throw new EventPublishException($"An error occurred while publishing an event of type {@event.Type} to the channel '{channel.GetType().Name}'", ex);
					}
				}
			}
		}

		public Task PublishAsync(Type dataType, object? data, CancellationToken cancellationToken = default) {
			CloudEvent @event;

			try {
				@event = CreateEventFromData(dataType, data);
			} catch (Exception ex) {
				_logger.LogEventCreateError(ex, dataType);

				if (PublisherOptions.ThrowOnErrors)
					throw new EventPublishException($"An error occurred while creating an event of type {dataType.FullName} from the provided data", ex);

				return Task.CompletedTask;
			}
			
			return PublishEventAsync(@event, cancellationToken);
		}

        protected virtual CloudEvent CreateEventFromData(Type dataType, object? data)
        {
            if (EventCreator == null)
                throw new NotSupportedException("Cannot create events from the data");

			return EventCreator.CreateEventFromData(dataType, data, PublisherOptions.JsonSerializerOptions);
        }

        public Task PublishAsync<TData>(TData data, CancellationToken cancellationToken = default)
			=> PublishAsync(typeof(TData), data, cancellationToken);
	}
}
