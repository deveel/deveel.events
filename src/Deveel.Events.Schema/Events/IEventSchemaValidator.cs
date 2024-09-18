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

using CloudNative.CloudEvents;

using System.ComponentModel.DataAnnotations;

namespace Deveel.Events {
    /// <summary>
    /// A service that is used to validate an event against a schema
    /// that describes its structure.
    /// </summary>
    public interface IEventSchemaValidator {
        /// <summary>
        /// Validates the given event against the schema provided.
        /// </summary>
        /// <param name="schema">
        /// The schema that describes the structure of the event.
        /// </param>
        /// <param name="event">
        /// The instance of the event to validate.
        /// </param>
        /// <param name="cancellationToken">
        /// A token that can be used to cancel the validation.
        /// </param>
        /// <returns>
        /// Returns an asynchronous stream of validation results.
        /// </returns>
		IAsyncEnumerable<ValidationResult> ValidateEventAsync(IEventSchema schema, CloudEvent @event, CancellationToken cancellationToken = default);
	}
}
