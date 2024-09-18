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
    /// A set of properties (metadata) that are provided along 
    /// a message in the Azure ServiceBus.
    /// </summary>
    public static class ServiceBusMessageProperties {
        /// <summary>
        /// The version of the data that is being sent.
        /// </summary>
		public const string DataVersion = "event.dataVersion";

        /// <summary>
        /// The type of the event that is being sent.
        /// </summary>
		public const string EventType = "event.type";

        /// <summary>
        /// The timestamp of the event that is being sent.
        /// </summary>
		public const string TimeStamp = "event.timestamp";
	}
}
