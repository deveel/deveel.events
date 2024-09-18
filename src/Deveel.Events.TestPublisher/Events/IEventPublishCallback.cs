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

namespace Deveel.Events {
    /// <summary>
    /// A callback that is invoked when an event is published.
    /// </summary>
    public interface IEventPublishCallback {
        /// <summary>
        /// Invoked when an event is published.
        /// </summary>
        /// <param name="event">The event that was published.</param>
        /// <returns>
        /// Returns a <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
		Task OnEventPublishedAsync(CloudEvent @event);
	}
}
