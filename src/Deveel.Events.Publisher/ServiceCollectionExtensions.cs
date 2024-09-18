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

using Deveel.Events;

using Microsoft.Extensions.DependencyInjection;

namespace Deveel {
    /// <summary>
    /// Extensions for the <see cref="IServiceCollection"/> to add the event publisher
	/// into the service collection of an application.
    /// </summary>
    public static class ServiceCollectionExtensions {
        /// <summary>
        /// Adds the event publisher into the service collection of an application.
        /// </summary>
        /// <param name="services">
		/// The service collection to add the event publisher into.
		/// </param>
        /// <returns>
		/// Returns an instance of the <see cref="EventPublisherBuilder"/> that can be used
		/// to configure the event publisher.
		/// </returns>
        public static EventPublisherBuilder AddEventPublisher(this IServiceCollection services) {
			return new EventPublisherBuilder(services);
		}

        /// <summary>
        /// Adds the event publisher into the service collection of an application,
        /// using the configuration section with the given path to configure the publisher.
        /// </summary>
        /// <param name="services">
        /// The service collection to add the event publisher into.
        /// </param>
        /// <param name="sectionPath">
        /// The path to the configuration section that contains the settings for the 
        /// event publisher.
        /// </param>
        /// <returns>
        /// Returns an instance of the <see cref="EventPublisherBuilder"/> that can be used
        /// to configure the event publisher.
        /// </returns>
        public static EventPublisherBuilder AddEventPublisher(this IServiceCollection services, string sectionPath) {
			return new EventPublisherBuilder(services)
				.Configure(sectionPath);
		}

        /// <summary>
        /// Adds the event publisher into the service collection of an application,
        /// using the given configuration delegate to configure the publisher.
        /// </summary>
        /// <param name="services">
        /// The service collection to add the event publisher into.
        /// </param>
        /// <param name="configure">
        /// The delegate that is used to configure the event publisher.
        /// </param>
        /// <returns>
        /// Returns an instance of the <see cref="EventPublisherBuilder"/> that can be used
        /// to configure the event publisher.
        /// </returns>
		public static EventPublisherBuilder AddEventPublisher(this IServiceCollection services, Action<EventPublisherOptions> configure) {
			return new EventPublisherBuilder(services)
				.Configure(configure);
		}
	}
}
