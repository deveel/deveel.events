using CloudNative.CloudEvents;

namespace Deveel.Events {
	public interface IEventPublishCallback {
		Task OnEventPublishedAsync(CloudEvent @event);
	}
}
