using System.ComponentModel.DataAnnotations;

namespace Deveel.Events {
	public interface IEventSchemaValidator {
		IAsyncEnumerable<ValidationResult> ValidateEventAsync(IEventSchema schema, IEvent @event, CancellationToken cancellationToken = default);
	}
}
