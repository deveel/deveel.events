namespace Deveel.Events {
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
	public sealed class EventPropertyAttribute : Attribute {
		public EventPropertyAttribute(string? name, string? schemaOrVersion = null) {
			if (!String.IsNullOrWhiteSpace(schemaOrVersion)) {
				if (System.Version.TryParse(schemaOrVersion, out _))
				{
					Version = schemaOrVersion;
				} else if (Uri.TryCreate(schemaOrVersion, UriKind.Absolute, out var uri))
				{
                    Schema = uri;
                } else
				{
                    throw new ArgumentException("The schema or version string is not valid", nameof(schemaOrVersion));
                }
			}

			Name = name;
			Version = schemaOrVersion;
		}

		public string? Name { get; }

		public string? Description { get; set; }

		public string? Version { get; set; }

		public Uri? Schema { get; set; }
	}
}
