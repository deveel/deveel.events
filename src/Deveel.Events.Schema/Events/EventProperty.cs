namespace Deveel.Events {
	public class EventProperty : IEventProperty, IVersionedElement {
		public EventProperty(string name, string dataType, string? version = null) {
			ArgumentNullException.ThrowIfNull(name, nameof(name));
			ArgumentNullException.ThrowIfNull(dataType, nameof(dataType));

			Version? propertyVersion = null;
			if (!String.IsNullOrWhiteSpace(version) &&
				!System.Version.TryParse(version, out propertyVersion))
				throw new ArgumentException("The version string is not valid", nameof(version));

			Name = name;
			DataType = dataType;
			Version = propertyVersion;

			Properties = new EventPropertyCollection(this);
			Constraints = new EventPropertyConstraintCollection();
		}

		public string Name { get; }

		public string? Description { get; set; }

		public string DataType { get; }

		string IEventProperty.Version => Version.ToString();

		public Version Version { get; internal set; }

		IEnumerable<IEventPropertyConstraint> IEventProperty.Constraints => Constraints;

		public EventPropertyConstraintCollection Constraints { get; }

		IEnumerable<IEventProperty> IEventProperty.Properties => Properties;

		public EventPropertyCollection Properties { get; }
	}
}
