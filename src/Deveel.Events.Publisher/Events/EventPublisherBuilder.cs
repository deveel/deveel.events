//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Deveel.Events {
	public sealed class EventPublisherBuilder {
		internal EventPublisherBuilder(IServiceCollection services) {
			Services = services;

			AddDefaultServices();
		}

		public IServiceCollection Services { get; }

		private void AddDefaultServices() {
			Services.TryAddSingleton<EventPublisher>();
			Services.TryAddSingleton<IEventIdGenerator>(EventGuidGenerator.Default);
			Services.TryAddSingleton<IEventSystemTime>(EventSystemTime.Instance);
			Services.TryAddSingleton<IEventCreator, EventCreator>();
		}

		public EventPublisherBuilder Configure(Action<EventPublisherOptions> configure) {
			Services.Configure(configure);

			return this;
		}

		public EventPublisherBuilder Configure(string sectionPath) {
			Services.AddOptions<EventPublisherOptions>()
				.BindConfiguration(sectionPath);

			return this;
		}

		public EventPublisherBuilder UsePublisher<TPublisher>(ServiceLifetime lifetime = ServiceLifetime.Singleton)
			where TPublisher : EventPublisher {
			Services.RemoveAll<EventPublisher>();
			Services.Add(new ServiceDescriptor(typeof(EventPublisher), typeof(TPublisher), lifetime));

			return this;
		}

		public EventPublisherBuilder UseGuid(string? format = null) {
			Services.RemoveAll<IEventIdGenerator>();
			Services.AddSingleton<IEventIdGenerator, EventGuidGenerator>();

			Services.AddOptions<EventGuidGeneratorOptions>()
				.Configure(o => o.Format = format);

			return this;
		}

		public EventPublisherBuilder UseSystemTime<TSystemTime>(ServiceLifetime lifetime = ServiceLifetime.Singleton)
			where TSystemTime : class, IEventSystemTime {
			Services.RemoveAll<IEventSystemTime>();
			Services.Add(new ServiceDescriptor(typeof(IEventSystemTime), typeof(TSystemTime), lifetime));

			return this;
		}
	}
}
