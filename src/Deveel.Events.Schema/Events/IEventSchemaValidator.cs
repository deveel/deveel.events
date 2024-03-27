using CloudNative.CloudEvents;

using System.ComponentModel.DataAnnotations;

namespace Deveel.Events {
	public interface IEventSchemaValidator {
		IAsyncEnumerable<ValidationResult> ValidateEventAsync(IEventSchema schema, CloudEvent @event, CancellationToken cancellationToken = default);
	}
}
