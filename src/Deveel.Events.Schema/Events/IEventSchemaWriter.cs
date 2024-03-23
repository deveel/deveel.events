namespace Deveel.Events {
	public interface IEventSchemaWriter {
		Task WriteToAsync(Stream stream, IEventSchema schema, CancellationToken cancellationToken);
	}
}
