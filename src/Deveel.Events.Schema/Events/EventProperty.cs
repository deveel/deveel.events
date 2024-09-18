namespace Deveel.Events {
    /// <summary>
    /// A property of an event that can be used to describe the data
	/// that is part of the event.
    /// </summary>
    public class EventProperty : IEventProperty, IVersionedElement {
        /// <summary>
        /// Constructs an event property with the given name, data type
		/// and optionally the version of the event this property belongs to.
        /// </summary>
        /// <param name="name">
		/// The name of the property that is used to identify it.
        /// </param>
        /// <param name="dataType">
		/// The data type that the property can hold.
		/// </param>
        /// <param name="version">
		/// The version of the event this property belongs to.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Thrown when the name or data type is <c>null</c>.
		/// </exception>
        /// <exception cref="ArgumentException">
		/// Thrown when the version string is provided and it is
		/// not a valid version.
		/// </exception>
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

		/// <inheritdoc/>
		public string Name { get; }

        /// <inheritdoc/>
        public string? Description { get; set; }

        /// <inheritdoc/>
        public string DataType { get; }

		string IEventProperty.Version => Version.ToString();

        /// <inheritdoc/>
        public Version Version { get; internal set; }

		IEnumerable<IEventPropertyConstraint> IEventProperty.Constraints => Constraints;

        /// <inheritdoc/>
        public EventPropertyConstraintCollection Constraints { get; }

		IEnumerable<IEventProperty> IEventProperty.Properties => Properties;

		/// <inheritdoc/>
        public EventPropertyCollection Properties { get; }
	}
}
