﻿//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using RabbitMQ.Client;

namespace Deveel.Events
{
    /// <summary>
    /// Extends the <see cref="EventPublisherBuilder"/> to add support for
    /// the RabbitMQ event publishing channel.
    /// </summary>
    public static class EventPublisherBuilderExtensions
    {
        private static EventPublisherBuilder AddRabbitMqChannel(this EventPublisherBuilder builder)
        {
            builder.Services.AddSingleton<IEventPublishChannel, RabbitMqEventPublishChannel>();
            builder.Services.TryAddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
            builder.Services.TryAddSingleton<IRabbitMqMessageFactory, RabbitMqMessageFactory>();
            builder.Services.TryAddSingleton<IConnection>(sp =>
            {
                var factory = sp.GetRequiredService<IRabbitMqConnectionFactory>();
                return factory.CreateConnection();
            });

            return builder;
        }

        /// <summary>
        /// Adds the RabbitMQ event publishing channel to the event publisher.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="EventPublisherBuilder"/> to add the channel to.
        /// </param>
        /// <param name="configure">
        /// An action to configure the options for the RabbitMQ channel.
        /// </param>
        /// <returns>
        /// Returns the <see cref="EventPublisherBuilder"/> to continue the configuration.
        /// </returns>
        public static EventPublisherBuilder UseRabbitMq(this EventPublisherBuilder builder, Action<RabbitMqEventPublishChannelOptions> configure)
        {
            builder.Services.AddOptions<RabbitMqEventPublishChannelOptions>()
                .Configure(configure);

            return builder.AddRabbitMqChannel();
        }

        /// <summary>
        /// Adds the RabbitMQ event publishing channel to the event publisher.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="EventPublisherBuilder"/> to add the channel to.
        /// </param>
        /// <param name="sectionPath">
        /// The configuration section path to bind the options for the RabbitMQ channel.
        /// </param>
        /// <returns>
        /// Returns the <see cref="EventPublisherBuilder"/> to continue the configuration.
        /// </returns>
        public static EventPublisherBuilder UseRabbitMq(this EventPublisherBuilder builder, string sectionPath)
        {
            builder.Services.AddOptions<RabbitMqEventPublishChannelOptions>()
                .BindConfiguration(sectionPath);

            return builder.AddRabbitMqChannel();
        }
    }
}
