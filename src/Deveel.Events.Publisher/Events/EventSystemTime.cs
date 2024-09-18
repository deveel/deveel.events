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

namespace Deveel.Events {
    /// <summary>
    /// A default implementation of <see cref="IEventSystemTime"/> that
    /// is based on the system clock.
    /// </summary>
    public sealed class EventSystemTime : IEventSystemTime {
        /// <inheritdoc/>
		public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

        /// <summary>
        /// The default instance of the <see cref="EventSystemTime"/>.
        /// </summary>
		public static readonly EventSystemTime Instance = new EventSystemTime();
	}
}
