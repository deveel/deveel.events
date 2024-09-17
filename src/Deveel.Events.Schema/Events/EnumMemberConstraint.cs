namespace Deveel.Events {
    /// <summary>
    /// A constraint that is used to restrict the values of a
	/// property to a set of allowed ones.
    /// </summary>
    /// <typeparam name="TValue">
	/// The type of values that allowed by the constraint.
	/// </typeparam>
    public class EnumMemberConstraint<TValue> : IEventPropertyConstraint {
        /// <summary>
        /// Constructs a constraint that allows only the values
        /// enumerated in the given list.
        /// </summary>
        /// <param name="allowedValues">
        /// The list of values that are allowed by the constraint.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Throws when the list of allowed values is <c>null</c>.
        /// </exception>
        public EnumMemberConstraint(IReadOnlyList<TValue> allowedValues) {
			AllowedValues = allowedValues ?? throw new ArgumentNullException(nameof(allowedValues));
		}

        /// <summary>
        /// The list of values that are allowed by the constraint.
        /// </summary>
		public IReadOnlyList<TValue> AllowedValues { get; }

		bool IEventPropertyConstraint.IsValid(object? value) {
			return value == null ? false : value is TValue enumValue ? AllowedValues.Contains(enumValue) : false;
		}
	}
}
