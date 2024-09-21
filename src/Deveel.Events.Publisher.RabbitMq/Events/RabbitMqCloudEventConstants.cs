//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

namespace Deveel.Events
{
    /// <summary>
    /// Contains the constants used in the RabbitMQ Cloud Event publishing.
    /// </summary>
    public static class RabbitMqCloudEventConstants
    {
        /// <summary>
        /// The attribute name used to define the exchange name 
        /// in the Cloud Event.
        /// </summary>
        public const string AmqpExchangeNameAttribute = "amqpexchange";

        /// <summary>
        /// The attribute name used to define the routing key
        /// in the Cloud Event.
        /// </summary>
        public const string AmqpRoutingKeyAttribute = "amqproutingkey";
    }
}
