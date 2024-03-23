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
			IEventIdGenerator? idGenerator = null,
			IEventSystemTime? systemTime = null,
			ILogger<EventPublisher>? logger = null) {
			_channels = channels;
			_idGenerator = idGenerator ?? EventGuidGenerator.Default;
			_systemTime = systemTime ?? EventSystemTime.Instance;
			_logger = logger ?? NullLogger<EventPublisher>.Instance;
			PublisherOptions = options.Value;
		}

		protected EventPublisherOptions PublisherOptions { get; }

		protected virtual Task PublishEventAsync(IEventPublishChannel channel, IEvent @event, CancellationToken cancellationToken) {
			return channel.PublishAsync(@event, cancellationToken);
		}

		protected virtual IEvent SetTimeStamp(IEvent @event) {
			if (@event.TimeStamp == null)
				@event = @event.WithTimeStamp(_systemTime.UtcNow);

			return @event;
		}

		protected virtual IEvent SetSource(IEvent @event) {
			if (@event.Source == null && PublisherOptions.Source != null)
				@event = @event.WithSource(PublisherOptions.Source);

			return @event;
		}

		protected virtual IEvent SetEventId(IEvent @event) {
			if (@event.EventId == null)
				@event = @event.WithEventId(_idGenerator.GenerateId());

			return @event;
		}

		protected virtual IEvent SetAttributes(IEvent @event) {
			if (PublisherOptions.Attributes != null)
				@event = @event.With(PublisherOptions.Attributes);

			return @event;
		}

		public async Task PublishEventAsync(IEvent @event, CancellationToken cancellationToken = default) {
			// TODO: validate the event before publishing

			IEvent eventToPublish = @event;
			@event = SetEventId(@event);
			@event = SetTimeStamp(@event);
			@event = SetSource(@event);
			@event = SetAttributes(@event);

			foreach (var channel in _channels) {
				_logger.TraceEventPublishing(@event.EventType, channel.GetType());

				try {
					await PublishEventAsync(channel, @event, cancellationToken);

					_logger.TraceEventPublished(@event.EventType, channel.GetType());
				} catch (Exception ex) {
					_logger.LogEventPublishError(ex, @event.EventType, channel.GetType());
					
					if (PublisherOptions.ThrowOnErrors) {
						throw new EventPublishException($"An error occurred while publishing an event of type {@event.EventType} to the channel '{channel.GetType().Name}'", ex);
					}
				}
			}
		}

		public Task PublishAsync(Type dataType, object? data, CancellationToken cancellationToken = default) {
			IEvent @event;

			try {
				@event = Event.FromData(dataType, data);
			} catch (Exception ex) {
				_logger.LogEventCreateError(ex, dataType);

				if (PublisherOptions.ThrowOnErrors)
					throw new EventPublishException($"An error occurred while creating an event of type {dataType.FullName} from the provided data", ex);

				return Task.CompletedTask;
			}
			
			return PublishEventAsync(@event, cancellationToken);
		}

		public Task PublishAsync<TData>(TData data, CancellationToken cancellationToken = default)
			=> PublishAsync(typeof(TData), data, cancellationToken);
	}
}
