using CloudNative.CloudEvents;

using System.Text.Json;

namespace Deveel.Events
{
    public static class EventCreatorExtensions
    {
        public static CloudEvent CreateEventFromData<T>(this IEventCreator creator, T data)
            => creator.CreateEventFromData(typeof(T), data);
    }
}
