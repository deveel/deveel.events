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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Deveel.Events {
	public static class EventPublisherBuilderExtensions {
		private static EventPublisherBuilder AddServiceBusChannel(this EventPublisherBuilder builder) {
			builder.Services.AddSingleton<IEventPublishChannel, ServiceBusEventPublishChannel>();
			builder.Services.TryAddSingleton<IServiceBusClientFactory, ServiceBusClientFactory>();
			builder.Services.TryAddSingleton<ServiceBusMessageFactory>();
			return builder;
		}

		public static EventPublisherBuilder AddServiceBusChannel(this EventPublisherBuilder builder, string sectionPath) {
			builder.AddServiceBusChannel();
			builder.Services.AddOptions<ServiceBusEventPublishChannelOptions>()
				.BindConfiguration(sectionPath)
				.PostConfigure<IOptions<EventPublisherOptions>>(ConfigureIdentifier);

			return builder;
		}

		public static EventPublisherBuilder AddServiceBusChannel(this EventPublisherBuilder builder, Action<ServiceBusEventPublishChannelOptions> configure) {
			builder.AddServiceBusChannel();
			builder.Services.AddOptions<ServiceBusEventPublishChannelOptions>()
				.Configure(configure)
				.PostConfigure<IOptions<EventPublisherOptions>>(ConfigureIdentifier);

			return builder;
		}

		private static void ConfigureIdentifier(ServiceBusEventPublishChannelOptions channelOptions, IOptions<EventPublisherOptions> publisherOptions) { 
			if (String.IsNullOrWhiteSpace(channelOptions.ClientOptions.Identifier))
				channelOptions.ClientOptions.Identifier = publisherOptions?.Value.Source?.ToString() ?? "";
		}
	}
}
