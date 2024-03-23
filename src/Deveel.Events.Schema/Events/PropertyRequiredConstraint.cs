namespace Deveel.Events {
	public sealed class PropertyRequiredConstraint : IEventPropertyConstraint {
		bool IEventPropertyConstraint.IsValid(object? value) => value != null;
	}
}
