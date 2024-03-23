using Deveel.Events;

using Microsoft.Extensions.DependencyInjection;

namespace Deveel {
	public static class ServiceCollectionExtensions {
		public static EventPublisherBuilder AddEventPublisher(this IServiceCollection services) {
			return new EventPublisherBuilder(services);
		}

		public static EventPublisherBuilder AddEventPublisher(this IServiceCollection services, string sectionPath) {
			return new EventPublisherBuilder(services)
				.Configure(sectionPath);
		}

		public static EventPublisherBuilder AddEventPublisher(this IServiceCollection services, Action<EventPublisherOptions> configure) {
			return new EventPublisherBuilder(services)
				.Configure(configure);
		}
	}
}
