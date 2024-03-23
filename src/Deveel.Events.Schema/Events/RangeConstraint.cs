namespace Deveel.Events {
	public sealed class RangeConstraint<TValue> : IEventPropertyConstraint where TValue : struct {
		public RangeConstraint(TValue? min, TValue? max) {
			if (min == null && max == null)
				throw new ArgumentException("At least one of the min or max values must be specified");

			Min = min;
			Max = max;
		}

		public TValue? Min { get; }

		public TValue? Max { get; }

		bool IEventPropertyConstraint.IsValid(object? value) {
			if (value == null)
				return Min == null && Max == null;

			if (value is TValue typedValue) {
				var comparer = Comparer<TValue>.Default;
				if (Min != null && Max != null)
					return comparer.Compare(typedValue, Min.Value) > 0 
						&& comparer.Compare(typedValue, Max.Value) < 0;

				if (Min != null && comparer.Compare(typedValue, Min.Value) > 0)
					return true;
				if (Max != null && comparer.Compare(typedValue, Max.Value) < 0)
					return true;
			}

			return false;
		}
	}
}
