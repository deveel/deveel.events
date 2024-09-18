//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

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
