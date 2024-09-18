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
    /// Extensions for the <see cref="IEventProperty"/> contract.
    /// </summary>
    public static class EventPropertyExtensions {
        /// <summary>
        /// Checks if the property is required.
        /// </summary>
        /// <param name="property">
        /// The property to check if it is required.
        /// </param>
        /// <returns>
        /// Returns <c>true</c> if the property is required, otherwise <c>false</c>.
        /// </returns>
		public static bool IsRequired(this IEventProperty property)
			=> property.Constraints?.OfType<PropertyRequiredConstraint>().Any() ?? false;
	}
}
