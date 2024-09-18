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
    /// An attribute that is used to mark a property of a
	/// type as a property of the payload of the event.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
	public sealed class EventPropertyAttribute : Attribute {
        /// <summary>
        /// Constructs an event property attribute with the given name
		/// and the optional schema or version of the event this property
		/// belongs to.
        /// </summary>
        /// <param name="name">
		/// The name of the property that is used to identify it
		/// within the event.
		/// </param>
        /// <param name="schemaOrVersion"></param>
        /// <exception cref="ArgumentException"></exception>
        public EventPropertyAttribute(string? name, string? schemaOrVersion = null) {
			if (!String.IsNullOrWhiteSpace(schemaOrVersion)) {
				if (System.Version.TryParse(schemaOrVersion, out _))
				{
					Version = schemaOrVersion;
				} else if (Uri.TryCreate(schemaOrVersion, UriKind.Absolute, out var uri))
				{
                    Schema = uri;
                } else
				{
                    throw new ArgumentException("The schema or version string is not valid", nameof(schemaOrVersion));
                }
			}

			Name = name;
			Version = schemaOrVersion;
		}

        /// <summary>
        /// The name of the property that is used to identify it
		/// within the event.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// A description of the property for documentation purposes.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The version of the event this property belongs to.
        /// </summary>
		public string? Version { get; set; }

        /// <summary>
        /// The schema of the data that is part of the property.
        /// </summary>
		public Uri? Schema { get; set; }
	}
}
