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

using CloudNative.CloudEvents;

namespace Deveel.Events
{
    /// <summary>
    /// A service that is used to publish events
    /// to one or more channels.
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Publishes the given event to the underlying channels.
        /// </summary>
        /// <param name="event">
        /// The instance of the <see cref="CloudEvent"/> to publish.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to cancel the operation.
        /// </param>
        /// <returns>
        /// Returns a task that will be completed when the event is published.
        /// </returns>
        Task PublishEventAsync(CloudEvent @event, CancellationToken cancellationToken = default);
    }
}
