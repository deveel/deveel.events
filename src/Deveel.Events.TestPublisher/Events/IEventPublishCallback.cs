namespace Deveel.Events {
	public interface IEventPublishCallback {
		Task OnEventPublishedAsync(IEvent @event);
	}
}
