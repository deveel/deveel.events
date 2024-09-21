//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using CloudNative.CloudEvents;

using Microsoft.Extensions.DependencyInjection;

namespace Deveel.Events {
    /// <summary>
    /// Extensions for the <see cref="EventPublisherBuilder"/> to add a test channel
	/// for publishing events.
    /// </summary>
    public static class EventPublisherBuilderExtensions {
        /// <summary>
        /// Adds a test channel to the event publisher that will
        /// be used to publish events.
        /// </summary>
        /// <param name="builder">
        /// The instance of the <see cref="EventPublisherBuilder"/> to add the 
        /// test channel to.
        /// </param>
        /// <param name="callback">
        /// A callback that is invoked when an event is published.
        /// </param>
        /// <returns>
        /// Returns the instance of the <see cref="EventPublisherBuilder"/> with the
        /// test channel added.
        /// </returns>
        public static EventPublisherBuilder AddTestChannel(this EventPublisherBuilder builder, IEventPublishCallback callback) {
			builder.Services.AddSingleton<IEventPublishChannel, TestEventPublishChannel>();
			builder.Services.AddSingleton<IEventPublishCallback>(callback);

			return builder;
		}

        /// <summary>
        /// Adds a test channel to the event publisher that will
        /// be used to publish events.
        /// </summary>
        /// <param name="builder">
        /// The instance of the <see cref="EventPublisherBuilder"/> to add the 
        /// test channel to.
        /// </param>
        /// <param name="callback">
        /// A callback that is invoked when an event is published.
        /// </param>
        /// <returns>
        /// Returns the instance of the <see cref="EventPublisherBuilder"/> with the
        /// test channel added.
        /// </returns>
        /// <seealso cref="AddTestChannel(EventPublisherBuilder, IEventPublishCallback)"/>
        public static EventPublisherBuilder AddTestChannel(this EventPublisherBuilder builder, Func<CloudEvent, Task> callback)
			=> AddTestChannel(builder, new DelegatedEventPublishCallback(callback));

        /// <summary>
        /// Adds a test channel to the event publisher that will
        /// be used to publish events.
        /// </summary>
        /// <param name="builder">
        /// The instance of the <see cref="EventPublisherBuilder"/> to add the 
        /// test channel to.
        /// </param>
        /// <param name="callback">
        /// A callback that is invoked when an event is published.
        /// </param>
        /// <returns>
        /// Returns the instance of the <see cref="EventPublisherBuilder"/> with the
        /// test channel added.
        /// </returns>
        /// <seealso cref="AddTestChannel(EventPublisherBuilder, IEventPublishCallback)"/>
        public static EventPublisherBuilder AddTestChannel(this EventPublisherBuilder builder, Action<CloudEvent> callback)
			=> AddTestChannel(builder, new DelegatedEventPublishCallback(callback));
	}
}
