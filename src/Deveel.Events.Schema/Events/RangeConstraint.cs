// 
//  Copyright 2023-2024 Antonello Provenzano
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

namespace Deveel.Events {
    /// <summary>
    /// A constraint that is used to restrict the values of a
	/// property to a range of values.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public sealed class RangeConstraint<TValue> : IEventPropertyConstraint where TValue : struct {
        /// <summary>
        /// Constructs a constraint that allows only the values
		/// within the given range.
        /// </summary>
        /// <param name="min">
		/// The minimum value that is allowed by the constraint,
		/// or <c>null</c> if there should be no minimum.
		/// </param>
        /// <param name="max">
		/// The maximum value that is allowed by the constraint,
		/// or <c>null</c> if there should be no maximum.
		/// </param>
        /// <exception cref="ArgumentException">
		/// Thrown when both the minimum and maximum values are <c>null</c>.
		/// </exception>
        public RangeConstraint(TValue? min, TValue? max) {
			if (min == null && max == null)
				throw new ArgumentException("At least one of the min or max values must be specified");

			Min = min;
			Max = max;
		}

        /// <summary>
        /// The minimum value that is allowed by the constraint,
		/// or <c>null</c> if there should be no minimum.
        /// </summary>
        public TValue? Min { get; }

        /// <summary>
        /// The maximum value that is allowed by the constraint,
		/// or <c>null</c> if there should be no maximum.
        /// </summary>
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
