using System.Collections.ObjectModel;

namespace Deveel.Events {
	public sealed class EventPropertyCollection : Collection<EventProperty> {
		private readonly IVersionedElement _owner;

		internal EventPropertyCollection(IVersionedElement owner) {
			_owner = owner;
		}

		public EventProperty this[string name] {
			get {
				ArgumentNullException.ThrowIfNull(name, nameof(name));
				return base.Items.First(x => x.Name == name);
			}
			set {
				for (var i = 0; i < base.Items.Count; i++) {
					if (base.Items[i].Name == name) {
						SetItem(i, value);
						return;
					}
				}

				throw new KeyNotFoundException($"The property {name} does not exist in the collection");
			}
		}

		public bool Contains(string name) {
			return base.Items.Any(x => x.Name == name);
		}

		protected override void InsertItem(int index, EventProperty item) {
			ArgumentNullException.ThrowIfNull(item, nameof(item));

			if (item.Version == null)
				item.Version = _owner.Version;

			if (item.Version > _owner.Version)
				throw new ArgumentException("The version of the property is not compatible with the owner", nameof(item));

			if (Contains(item.Name))
				throw new ArgumentException("The property with the same name already exists", nameof(item));

			base.InsertItem(index, item);
		}

		protected override void SetItem(int index, EventProperty item) {
			ArgumentNullException.ThrowIfNull(item, nameof(item));

			if (item.Version == null)
				item.Version = _owner.Version;

			if (item.Version > _owner.Version)
				throw new ArgumentException("The version of the property is not compatible with owner", nameof(item));

			for (var i = 0; i < base.Items.Count; i++) {
				if (i == index)
					continue;

				if (base.Items[i].Name == item.Name)
					throw new ArgumentException("The property with the same name already exists", nameof(item));
			}

			base.SetItem(index, item);
		}
	}
}
