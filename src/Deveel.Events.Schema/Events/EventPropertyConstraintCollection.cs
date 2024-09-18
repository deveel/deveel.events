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

using System.Collections.ObjectModel;

namespace Deveel.Events {
    /// <summary>
    /// A specialized collection of constraints that can be applied to
	/// an event property.
    /// </summary>
    public sealed class EventPropertyConstraintCollection : Collection<IEventPropertyConstraint> {
		internal EventPropertyConstraintCollection() {
		}

		/// <inheritdoc/>
		protected override void InsertItem(int index, IEventPropertyConstraint item) {
			ArgumentNullException.ThrowIfNull(item, nameof(item));

			var constraintType = item.GetType();
			if (base.Items.Any(x => x.GetType() == constraintType))
				throw new ArgumentException("The constraint of the same type already exists", nameof(item));

			base.InsertItem(index, item);
		}

        /// <inheritdoc/>
        protected override void SetItem(int index, IEventPropertyConstraint item) {
			ArgumentNullException.ThrowIfNull(item, nameof(item));

			var constraintType = item.GetType();
			for (var i = 0; i < base.Items.Count; i++) {
				if (i == index)
					continue;

				if (base.Items[i].GetType() == constraintType)
					throw new ArgumentException("The constraint of the same type already exists", nameof(item));
			}

			base.SetItem(index, item);
		}
	}
}
