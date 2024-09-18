//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using CloudNative.CloudEvents;

using Microsoft.Extensions.DependencyInjection;

namespace Deveel.Events {
	public static class EventPublisherBuilderExtensions {
		public static EventPublisherBuilder AddTestChannel(this EventPublisherBuilder builder, IEventPublishCallback callback) {
			builder.Services.AddSingleton<IEventPublishChannel, TestEventPublishChannel>();
			builder.Services.AddSingleton<IEventPublishCallback>(callback);

			return builder;
		}

		public static EventPublisherBuilder AddTestChannel(this EventPublisherBuilder builder, Func<CloudEvent, Task> callback)
			=> AddTestChannel(builder, new DelegatedEventPublishCallback(callback));

		public static EventPublisherBuilder AddTestChannel(this EventPublisherBuilder builder, Action<CloudEvent> callback)
			=> AddTestChannel(builder, new DelegatedEventPublishCallback(callback));
	}
}
