using System.Runtime.Serialization;

using Azure.Messaging.ServiceBus;

using CloudNative.CloudEvents;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Deveel.Events {
	public sealed class ServiceBusEventPublishChannel : IEventPublishChannel, IAsyncDisposable, IDisposable {
		private ServiceBusSender? sender;
		private ServiceBusClient? client;
		private readonly ServiceBusMessageFactory messageCreator;
		private readonly ILogger logger;

		private bool disposed;

		public ServiceBusEventPublishChannel(
			IOptions<ServiceBusEventPublishChannelOptions> options,
			IServiceBusClientFactory clientFactory,
			ServiceBusMessageFactory messageCreator,
			ILogger<ServiceBusEventPublishChannel>? logger = null)
			: this(options.Value, clientFactory, messageCreator, logger) {
		}

		private ServiceBusEventPublishChannel(
			ServiceBusEventPublishChannelOptions options,
			IServiceBusClientFactory clientFactory,
			ServiceBusMessageFactory messageCreator,
			ILogger<ServiceBusEventPublishChannel>? logger = null) {
			if (String.IsNullOrWhiteSpace(options.ConnectionString))
				throw new ArgumentException("The connection string is required");
			if (String.IsNullOrWhiteSpace(options.QueueName))
				throw new ArgumentException("The name of the queue is missing");

			var clientOptions = options.ClientOptions ?? new ServiceBusClientOptions();

			client = clientFactory.CreateClient(options.ConnectionString, clientOptions);

			sender = client.CreateSender(options.QueueName);
			this.messageCreator = messageCreator;
			this.logger = logger ?? NullLogger<ServiceBusEventPublishChannel>.Instance;
		}

		private void ThrowIfDisposed() {
			if (disposed)
				throw new ObjectDisposedException(nameof(ServiceBusEventPublishChannel));
		}

		public async Task PublishAsync(CloudEvent @event, CancellationToken cancellationToken = default) {
			ThrowIfDisposed();
			cancellationToken.ThrowIfCancellationRequested();

			ArgumentNullException.ThrowIfNull(@event, nameof(@event));

            logger.TracePublishingEvent(@event.Type);

			try {
				await sender!.SendMessageAsync(messageCreator.CreateMessage(@event), cancellationToken);
			} catch (ServiceBusException ex) {
				logger.LogErrorPublishingEvent(ex, @event.Type);
				throw new EventPublishException("The ServiceBus service caused an error", ex);
			} catch (SerializationException ex) {
				logger.LogErrorPublishingEvent(ex, @event.Type);
				throw new EventPublishException("It was not possible to serialize the message", ex);
			} catch (Exception ex) {
				logger.LogErrorPublishingEvent(ex, @event.Type);
				throw;
			}

		}

		public void Dispose() {
			if (disposed)
				return;

			DisposeAsyncCore().GetAwaiter().GetResult();
		}

		public async ValueTask DisposeAsync() {
			if (disposed)
				return;

			await DisposeAsyncCore();

			disposed = true;
		}

		private async Task DisposeAsyncCore() {
			if (sender != null) {
				await sender.DisposeAsync();
				sender = null;
			}

			if (client != null) {
				await client.DisposeAsync();
				client = null;
			}
		}
	}
}
