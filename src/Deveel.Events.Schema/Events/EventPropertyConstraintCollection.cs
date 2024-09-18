//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

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
