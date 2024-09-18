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
