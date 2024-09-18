//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using Azure.Messaging.ServiceBus;

namespace Deveel.Events {
    /// <summary>
    /// The options for a channel that publishes events 
    /// to an Azure Service Bus.
    /// </summary>
    public class ServiceBusEventPublishChannelOptions {
        /// <summary>
        /// Gets or sets the connection string to the Azure Service Bus
        /// instance that is used to publish the events.
        /// </summary>
		public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the queue in the Azure Service Bus
        /// where the events are published.
        /// </summary>
		public string QueueName { get; set; }

        /// <summary>
        /// Gets or sets the options for the client that connects to the
        /// Azure Service Bus.
        /// </summary>
		public ServiceBusClientOptions ClientOptions { get; set; } = new ServiceBusClientOptions();
	}
}
