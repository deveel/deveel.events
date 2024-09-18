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
    /// The interface that defines a property of 
	/// an event that can be used to describe the data
	/// that is part of the event.
    /// </summary>
    public interface IEventProperty {
        /// <summary>
        /// The name used to identify the property
        /// within the event.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// A description of the property that can be used
        /// for documentation purposes.
        /// </summary>
		string? Description { get; }

        /// <summary>
        /// The data type that the property can hold.
        /// </summary>
		string DataType { get; }

        /// <summary>
        /// The version of the event this property 
        /// belongs to.
        /// </summary>
		string Version { get; }

        /// <summary>
        /// The constraints that are applied to the property
        /// to restrict the values that can be assigned to it.
        /// </summary>
		IEnumerable<IEventPropertyConstraint> Constraints { get; }

        /// <summary>
        /// The collection of properties that are part of this
        /// property, when the data type is complex.
        /// </summary>
		IEnumerable<IEventProperty> Properties { get; }
	}
}
