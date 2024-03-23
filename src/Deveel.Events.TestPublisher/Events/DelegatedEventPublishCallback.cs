namespace Deveel.Events {
	class DelegatedEventPublishCallback : IEventPublishCallback {
		private readonly Func<IEvent, Task>? _asyncCallback;
		private readonly Action<IEvent>? _callback;

		public DelegatedEventPublishCallback(Func<IEvent, Task> callback) {
			_asyncCallback = callback;
		}

		public DelegatedEventPublishCallback(Action<IEvent> callback) {
			_callback = callback;
		}

		public Task OnEventPublishedAsync(IEvent @event) {
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
