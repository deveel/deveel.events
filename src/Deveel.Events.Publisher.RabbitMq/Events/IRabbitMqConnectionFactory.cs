﻿//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using RabbitMQ.Client;

namespace Deveel.Events
{
    /// <summary>
    /// A factory to create a connection to a RabbitMQ server
    /// to be used by the <see cref="RabbitMqEventPublishChannel"/>.
    /// </summary>
    public interface IRabbitMqConnectionFactory
    {
        /// <summary>
        /// Creates a new connection to the RabbitMQ server.
        /// </summary>
        /// <returns>
        /// Returns a new instance of <see cref="IConnection"/> that represents
        /// the connection to the RabbitMQ server.
        /// </returns>
        IConnection CreateConnection();
    }
}
