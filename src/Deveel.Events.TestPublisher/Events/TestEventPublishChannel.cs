namespace Deveel.Events {
	class TestEventPublishChannel : IEventPublishChannel {
		private readonly IEventPublishCallback _callback;

		public TestEventPublishChannel(IEventPublishCallback callback) {
			_callback = callback;
		}

		public Task PublishAsync(IEvent @event, CancellationToken cancellationToken = default) {
			return _callback.OnEventPublishedAsync(@event);
		}
	}
}
