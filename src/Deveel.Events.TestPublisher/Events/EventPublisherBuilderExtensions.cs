using Microsoft.Extensions.DependencyInjection;

namespace Deveel.Events {
	public static class EventPublisherBuilderExtensions {
		public static EventPublisherBuilder AddTestChannel(this EventPublisherBuilder builder, IEventPublishCallback callback) {
			builder.Services.AddSingleton<IEventPublishChannel, TestEventPublishChannel>();
			builder.Services.AddSingleton<IEventPublishCallback>(callback);

			return builder;
		}

		public static EventPublisherBuilder AddTestChannel(this EventPublisherBuilder builder, Func<IEvent, Task> callback)
			=> AddTestChannel(builder, new DelegatedEventPublishCallback(callback));

		public static EventPublisherBuilder AddTestChannel(this EventPublisherBuilder builder, Action<IEvent> callback)
			=> AddTestChannel(builder, new DelegatedEventPublishCallback(callback));
	}
}
