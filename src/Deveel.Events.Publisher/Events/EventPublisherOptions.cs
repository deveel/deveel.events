using System.Text.Json;

namespace Deveel.Events {
	public class EventPublisherOptions {
		public Uri? Source { get; set; }

		public bool ThrowOnErrors { get; set; } = false;

		public JsonSerializerOptions? JsonSerializerOptions { get; set; } = new JsonSerializerOptions();

		public Dictionary<string, object?> Attributes { get; set; } = new Dictionary<string, object?>();

		public Uri? DataSchemaBaseUri { get; set; }
	}
}
