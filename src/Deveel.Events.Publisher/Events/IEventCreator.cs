using CloudNative.CloudEvents;

using System.Text.Json;

namespace Deveel.Events
{
    public interface IEventCreator
    {
        CloudEvent CreateEventFromData(Type dataType, object? data, JsonSerializerOptions? jsonOptions = null);
    }
}
