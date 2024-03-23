namespace Deveel.Events {
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
	public sealed class EventPropertyAttribute : Attribute {
		public EventPropertyAttribute(string? name, string? version = null) {
			if (!String.IsNullOrWhiteSpace(version) &&
				!System.Version.TryParse(version, out _))
				throw new ArgumentException("The version string is not valid", nameof(version));

			Name = name;
			Version = version;
		}

		public string? Name { get; }

		public string? Description { get; set; }

		public string? Version { get; set; }
	}
}
