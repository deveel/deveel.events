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
    /// The contract that defines the schema of an event
	/// describing the properties and constraints that are
	/// contained in the event.
    /// </summary>
    public interface IEventSchema {
        /// <summary>
        /// The type of the event the schema is describing.
        /// </summary>
        string EventType { get; }

        /// <summary>
        /// The version of the event described by the schema.
        /// </summary>
		string Version { get; }

        /// <summary>
        /// A description of the event that can be used for
        /// documentation purposes.
        /// </summary>
		string? Description { get; }

        /// <summary>
        /// The content type of the event that is used to
        /// identify the format of the data.
        /// </summary>
		string ContentType { get; }

        /// <summary>
        /// The properties that are part of the event schema.
        /// </summary>
		IEnumerable<IEventProperty> Properties { get; }
	}
}
