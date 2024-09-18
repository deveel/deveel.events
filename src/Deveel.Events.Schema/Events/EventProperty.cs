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
    /// A property of an event that can be used to describe the data
	/// that is part of the event.
    /// </summary>
    public class EventProperty : IEventProperty, IVersionedElement {
        /// <summary>
        /// Constructs an event property with the given name, data type
		/// and optionally the version of the event this property belongs to.
        /// </summary>
        /// <param name="name">
		/// The name of the property that is used to identify it.
        /// </param>
        /// <param name="dataType">
		/// The data type that the property can hold.
		/// </param>
        /// <param name="version">
		/// The version of the event this property belongs to.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Thrown when the name or data type is <c>null</c>.
		/// </exception>
        /// <exception cref="ArgumentException">
		/// Thrown when the version string is provided and it is
		/// not a valid version.
		/// </exception>
        public EventProperty(string name, string dataType, string? version = null) {
			ArgumentNullException.ThrowIfNull(name, nameof(name));
			ArgumentNullException.ThrowIfNull(dataType, nameof(dataType));

			Version? propertyVersion = null;
			if (!String.IsNullOrWhiteSpace(version) &&
				!System.Version.TryParse(version, out propertyVersion))
				throw new ArgumentException("The version string is not valid", nameof(version));

			Name = name;
			DataType = dataType;
			Version = propertyVersion;

			Properties = new EventPropertyCollection(this);
			Constraints = new EventPropertyConstraintCollection();
		}

		/// <inheritdoc/>
		public string Name { get; }

        /// <inheritdoc/>
        public string? Description { get; set; }

        /// <inheritdoc/>
        public string DataType { get; }

		string IEventProperty.Version => Version.ToString();

        /// <inheritdoc/>
        public Version Version { get; internal set; }

		IEnumerable<IEventPropertyConstraint> IEventProperty.Constraints => Constraints;

        /// <inheritdoc/>
        public EventPropertyConstraintCollection Constraints { get; }

		IEnumerable<IEventProperty> IEventProperty.Properties => Properties;

		/// <inheritdoc/>
        public EventPropertyCollection Properties { get; }
	}
}
