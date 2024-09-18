//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

namespace Deveel.Events {
	public class EventPublishException : Exception {
		public EventPublishException() {
		}

		public EventPublishException(string? message) : base(message) {
		}

		public EventPublishException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}
