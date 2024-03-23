namespace Deveel.Events {
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public class EventAttributesAttribute : Attribute {
		public EventAttributesAttribute(string attributeName, object? value) {
			ArgumentNullException.ThrowIfNull(attributeName, nameof(attributeName));

			AttributeName = attributeName;
			Value = value;
		}

		public string AttributeName { get; }

		public object? Value { get; }
	}
}
