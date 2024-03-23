namespace Deveel.Events {
	public interface IEventPropertyConstraint {
		bool IsValid(object? value);
	}
}
