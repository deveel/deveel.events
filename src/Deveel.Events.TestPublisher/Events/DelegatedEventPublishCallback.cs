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
