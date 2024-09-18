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
    /// A service that is used to write an event schema to a stream
    /// in a specific format.
    /// </summary>
    public interface IEventSchemaWriter {
        /// <summary>
        /// Writes the schema to the given stream in the format
        /// that is specific to the implementation.
        /// </summary>
        /// <param name="stream">
        /// The stream to write the schema to.
        /// </param>
        /// <param name="schema">
        /// The instance of the schema to write.
        /// </param>
        /// <param name="cancellationToken">
        /// A token that can be used to cancel the writing.
        /// </param>
        /// <returns>
        /// Returns an asynchronous task that represents the writing
        /// of the schema to the stream.
        /// </returns>
		Task WriteToAsync(Stream stream, IEventSchema schema, CancellationToken cancellationToken);
	}
}
