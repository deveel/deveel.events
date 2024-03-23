namespace Deveel.Events {
	public interface IEventPublishChannel {
		Task PublishAsync(IEvent @event, CancellationToken cancellationToken = default);
	}
}
