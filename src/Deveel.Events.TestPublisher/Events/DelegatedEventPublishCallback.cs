//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using CloudNative.CloudEvents;

namespace Deveel.Events {
	class DelegatedEventPublishCallback : IEventPublishCallback {
		private readonly Func<CloudEvent, Task>? _asyncCallback;
		private readonly Action<CloudEvent>? _callback;

		public DelegatedEventPublishCallback(Func<CloudEvent, Task> callback) {
			_asyncCallback = callback;
		}

		public DelegatedEventPublishCallback(Action<CloudEvent> callback) {
			_callback = callback;
		}

		public Task OnEventPublishedAsync(CloudEvent @event) {
			if (_callback != null) {
				_callback(@event);
				return Task.CompletedTask;
			}

			if (_asyncCallback != null)
				return _asyncCallback(@event);

			return Task.CompletedTask;
		}
	}
}
