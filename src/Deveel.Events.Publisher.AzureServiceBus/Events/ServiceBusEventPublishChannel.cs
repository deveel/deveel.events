//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using System.Runtime.Serialization;

using Azure.Messaging.ServiceBus;

using CloudNative.CloudEvents;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Deveel.Events {
    /// <summary>
    /// A channel that publishes events to an Azure Service Bus queue.
    /// </summary>
    public sealed class ServiceBusEventPublishChannel : IEventPublishChannel, IAsyncDisposable, IDisposable {
		private ServiceBusSender? sender;
		private ServiceBusClient? client;
		private readonly ServiceBusMessageFactory messageCreator;
		private readonly ILogger logger;

		private bool disposed;

        /// <summary>
        /// Creates a new instance of the channel,
		/// using the specified options, client factory 
		/// and message creator.
        /// </summary>
        /// <param name="options">
		/// The options to configure the channel.
		/// </param>
        /// <param name="clientFactory">
		/// A factory to create the client to the Azure Service Bus.
		/// </param>
        /// <param name="messageCreator">
		/// The factory to create the message to send to the queue.
		/// </param>
        /// <param name="logger">
		/// A logger to record the operations of the channel.
		/// </param>
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public void Dispose() {
			if (disposed)
				return;

			DisposeAsyncCore().GetAwaiter().GetResult();
		}

        /// <inheritdoc />
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
