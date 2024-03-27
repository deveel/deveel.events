using CloudNative.CloudEvents;

namespace Deveel.Events {
	public interface IEventPublishChannel {
		Task PublishAsync(CloudEvent @event, CancellationToken cancellationToken = default);
	}
}
