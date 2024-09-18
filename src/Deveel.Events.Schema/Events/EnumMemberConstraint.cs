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
    /// A constraint that is used to restrict the values of a
	/// property to a set of allowed ones.
    /// </summary>
    /// <typeparam name="TValue">
	/// The type of values that allowed by the constraint.
	/// </typeparam>
    public class EnumMemberConstraint<TValue> : IEventPropertyConstraint {
        /// <summary>
        /// Constructs a constraint that allows only the values
        /// enumerated in the given list.
        /// </summary>
        /// <param name="allowedValues">
        /// The list of values that are allowed by the constraint.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Throws when the list of allowed values is <c>null</c>.
        /// </exception>
        public EnumMemberConstraint(IReadOnlyList<TValue> allowedValues) {
			AllowedValues = allowedValues ?? throw new ArgumentNullException(nameof(allowedValues));
		}

        /// <summary>
        /// The list of values that are allowed by the constraint.
        /// </summary>
		public IReadOnlyList<TValue> AllowedValues { get; }

		bool IEventPropertyConstraint.IsValid(object? value) {
			return value == null ? false : value is TValue enumValue ? AllowedValues.Contains(enumValue) : false;
		}
	}
}
