using CloudNative.CloudEvents;

namespace Deveel.Events
{
    public interface IEventFactory
    {
        CloudEvent CreateEvent();
    }
}
