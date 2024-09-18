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
    /// Provides a factory for creating events.
    /// </summary>
    /// <remarks>
    /// This contract is a provision for allowing
    /// the creation of events not using reflection.
    /// </remarks>
    public interface IEventFactory
    {
        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <returns>
        /// Returns a new <see cref="CloudEvent"/>.
        /// </returns>
        CloudEvent CreateEvent();
    }
}
