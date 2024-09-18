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
