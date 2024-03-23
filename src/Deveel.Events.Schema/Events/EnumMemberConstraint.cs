namespace Deveel.Events {
	public class EnumMemberConstraint<TValue> : IEventPropertyConstraint {
		public EnumMemberConstraint(IReadOnlyList<TValue> allowedValues) {
			AllowedValues = allowedValues ?? throw new ArgumentNullException(nameof(allowedValues));
		}

		public IReadOnlyList<TValue> AllowedValues { get; }

		bool IEventPropertyConstraint.IsValid(object? value) {
			return value == null ? false : value is TValue enumValue ? AllowedValues.Contains(enumValue) : false;
		}
	}
}
