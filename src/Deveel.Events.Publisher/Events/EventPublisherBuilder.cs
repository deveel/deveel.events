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
