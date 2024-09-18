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
    /// A service that creates a <see cref="CloudEvent"/> from a given data object.
    /// </summary>
    /// <remarks>
    /// Implementations of this interface are typically using
    /// reflection to extract the metadata of the event from the
    /// object and create a <see cref="CloudEvent"/> instance.
    /// </remarks>
    public interface IEventCreator
    {
        /// <summary>
        /// Creates a <see cref="CloudEvent"/> from the given 
        /// data object.
        /// </summary>
        /// <param name="dataType">
        /// The type of the data object.
        /// </param>
        /// <param name="data">
        /// The data object that is transported by the event.
        /// </param>
        /// <returns>
        /// Returns a <see cref="CloudEvent"/> instance that is
        /// built from the given data object.
        /// </returns>
        CloudEvent CreateEventFromData(Type dataType, object? data);
    }
}
