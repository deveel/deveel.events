using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Deveel.Events {
	public static class EventPublisherBuilderExtensions {
		private static EventPublisherBuilder AddServiceBusChannel(this EventPublisherBuilder builder) {
			builder.Services.AddSingleton<IEventPublishChannel, ServiceBusEventPublishChannel>();
			builder.Services.TryAddSingleton<IServiceBusClientFactory, ServiceBusClientFactory>();
			builder.Services.TryAddSingleton<ServiceBusMessageCreator>();
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
				channelOptions.ClientOptions.Identifier = publisherOptions?.Value.Source ?? "";
		}
	}
}
