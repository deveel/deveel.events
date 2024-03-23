using System.Collections.ObjectModel;

namespace Deveel.Events {
	public sealed class EventPropertyConstraintCollection : Collection<IEventPropertyConstraint> {
		internal EventPropertyConstraintCollection() {
		}

		protected override void InsertItem(int index, IEventPropertyConstraint item) {
			ArgumentNullException.ThrowIfNull(item, nameof(item));

			var constraintType = item.GetType();
			if (base.Items.Any(x => x.GetType() == constraintType))
				throw new ArgumentException("The constraint of the same type already exists", nameof(item));

			base.InsertItem(index, item);
		}

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
