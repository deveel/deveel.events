﻿// 
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
    /// A service used to create instances of <see cref="ServiceBusClient"/>
    /// for communication with Azure Service Bus.
    /// </summary>
    public interface IServiceBusClientFactory {
        /// <summary>
        /// Creates a new instance of <see cref="ServiceBusClient"/>
        /// from the given connection string and options.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string to the Azure Service Bus.
        /// </param>
        /// <param name="options">
        /// The options to use when creating the client.
        /// </param>
        /// <returns>
        /// Returns a new instance of <see cref="ServiceBusClient"/>
        /// that can be used to communicate with the Azure Service Bus.
        /// </returns>
		ServiceBusClient CreateClient(string connectionString, ServiceBusClientOptions options);
	}
}
