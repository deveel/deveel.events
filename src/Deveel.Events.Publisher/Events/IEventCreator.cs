using CloudNative.CloudEvents;

namespace Deveel.Events
{
    public interface IEventCreator
    {
        CloudEvent CreateEventFromData(Type dataType, object? data);
    }
}
