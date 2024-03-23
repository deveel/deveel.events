namespace Deveel.Events {
	public class EventPublisherOptions {
		public string? Source { get; set; }

		public bool ThrowOnErrors { get; set; } = false;

		public Dictionary<string, object?> Attributes { get; set; } = new Dictionary<string, object?>();
	}
}
