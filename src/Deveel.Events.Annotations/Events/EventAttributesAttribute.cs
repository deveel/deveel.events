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
    /// An attribute that can be used to describe additional attributes
	/// (such as metadata) that can be attached to an event
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public class EventAttributesAttribute : Attribute {
        /// <summary>
        /// Constructs an attribute with the given name and value.
        /// </summary>
        /// <param name="attributeName">
        /// The name of the attribute that is used to uniquely identify it
        /// within the event.
        /// </param>
        /// <param name="value">
        /// The constant value of the attribute that is being set.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the attribute name is <c>null</c>.
        /// </exception>
        public EventAttributesAttribute(string attributeName, object? value) {
			ArgumentNullException.ThrowIfNull(attributeName, nameof(attributeName));

			AttributeName = attributeName;
			Value = value;
		}

        /// <summary>
        /// Gets the name of the attribute that is used to uniquely 
        /// identify it within the event.
        /// </summary>
		public string AttributeName { get; }

        /// <summary>
        /// Gets the constant value of the attribute that is being set.
        /// </summary>
		public object? Value { get; }
	}
}
