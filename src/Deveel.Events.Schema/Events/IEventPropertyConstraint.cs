//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

namespace Deveel.Events {
    /// <summary>
    /// A constraint that is applied to a property of an event
    /// to restrict the values that can be assigned to it.
    /// </summary>
    public interface IEventPropertyConstraint {
        /// <summary>
        /// Validates the given value against the constraint
        /// defined by the implementation.
        /// </summary>
        /// <param name="value">
        /// The value to be validated.
        /// </param>
        /// <returns>
        /// Returns <c>true</c> if the value is valid, otherwise
        /// returns <c>false</c>.
        /// </returns>
		bool IsValid(object? value);
	}
}
