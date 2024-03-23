namespace Deveel.Events {
	public interface IEventProperty {
		string Name { get; }

		string? Description { get; }

		string DataType { get; }

		string Version { get; }

		IEnumerable<IEventPropertyConstraint> Constraints { get; }

		IEnumerable<IEventProperty> Properties { get; }
	}
}
