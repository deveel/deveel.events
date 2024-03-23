using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Deveel.Events {
	public readonly struct EventData {
		private readonly bool isEmpty;

		private EventData(bool isEmpty, object? content, EventContentType contentType) {
			if (isEmpty && content != null)
				throw new ArgumentException("The content of an empty event data must be null", nameof(content));
			if (isEmpty && contentType != EventContentType.None)
				throw new ArgumentException("The content type of an empty event data must be None", nameof(contentType));

			this.isEmpty = isEmpty;
			Content = NormalizeContent(content, contentType);
			ContentType = contentType;
		}

		private static object? NormalizeContent(object? content, EventContentType contentType) {
			return contentType switch {
				EventContentType.Json => NormalizeJsonContent(content),
				EventContentType.Binary => NormalizeBinaryContent(content),
				EventContentType.Object => content,
				EventContentType.None => null,
				_ => throw new NotSupportedException($"The content type '{contentType}' is not supported")
			};
		}
		private static byte[]? NormalizeBinaryContent(object? content) {
			return content switch {
				null => null,
				byte[] bytes => bytes,
				string str => Encoding.UTF8.GetBytes(str),
				JsonNode jsonNode => JsonSerializer.SerializeToUtf8Bytes(jsonNode.ToJsonString()),
				_ => null
			};
		}

		private static byte[]? NormalizeJsonContent(object? content) {
			return content switch {
				null => null,
				byte[] bytes => bytes,
				string str => Encoding.UTF8.GetBytes(str),
				JsonNode jsonNode => Encoding.UTF8.GetBytes(jsonNode.ToJsonString()),
				_ => throw new ArgumentException("The content of a JSON event data must be a byte array, a string or a JSON node", nameof(content))
			};
		}

		public EventData(ReadOnlySpan<byte> content)
			: this(content.ToArray()) {
		}

		public EventData(byte[] content)
			: this(false, content, EventContentType.Binary) {
		}

		public EventData(object? content, EventContentType contentType)
			: this(false, content, contentType) {
		}

		public EventData(object content, JsonSerializerOptions? jsonOptions = null)
			: this(false, JsonSerializer.SerializeToUtf8Bytes(content, content.GetType(), jsonOptions), EventContentType.Json) {
		}

		public EventData(object? content)
			: this(false, content, EventContentType.Object) {
		}

		public object? Content { get; }

		public EventContentType ContentType { get; }

		public override int GetHashCode()
			=> isEmpty ? 0 : HashCode.Combine(Content);

		public override bool Equals(object? obj) {
			return obj is EventData data && this == data;
		}

		public static readonly EventData Empty = new EventData(true, null, EventContentType.None);

		public byte[]? AsBinary() {
			if (isEmpty || Content == null)
				return null;

			return ContentType switch {
				EventContentType.Binary => Content as byte[],
				EventContentType.Json => Content as byte[],
				EventContentType.Object => JsonSerializer.SerializeToUtf8Bytes(Content),
				EventContentType.None => null,
				_ => throw new NotSupportedException($"The content type '{ContentType}' is not supported for conversion to binary")
			};
		}

		public JsonNode? AsJsonNode(JsonSerializerOptions? jsonOptions = null) {
			if (isEmpty || Content == null)
				return null;

			return ContentType switch {
				EventContentType.Json => AsJsonNodeFromJson(Content, jsonOptions),
				EventContentType.Binary => JsonSerializer.Deserialize<JsonNode>(Content as byte[], jsonOptions),
				EventContentType.Object => JsonSerializer.Deserialize<JsonNode>(JsonSerializer.Serialize(Content, Content.GetType(), jsonOptions), jsonOptions),
				EventContentType.None => null,
				_ => throw new NotSupportedException($"The content type '{ContentType}' is not supported for conversion to JSON")
			};
		}

		private JsonNode? AsJsonNodeFromJson(object content, JsonSerializerOptions? jsonOptions) {
			if (content is byte[] bytes)
				return JsonSerializer.Deserialize<JsonNode>(bytes, jsonOptions);
			if (content is string str)
				return JsonSerializer.Deserialize<JsonNode>(str, jsonOptions);

			return null;
		}

		public string? AsJson(JsonSerializerOptions? jsonOptions = null) {
			var jsonNode = AsJsonNode(jsonOptions);
			return jsonNode?.ToJsonString(jsonOptions);
		}

		public T? AsObject<T>(JsonSerializerOptions? jsonOptions = null) {
			return (T?)AsObject(typeof(T), jsonOptions);
		}

		public object? AsObject(Type objectType, JsonSerializerOptions? jsonOptions = null) {
			if (isEmpty || Content == null)
				return default;

			return ContentType switch {
				EventContentType.Json => JsonSerializer.Deserialize(Content as byte[], objectType, jsonOptions),
				EventContentType.Object => ConvertToObject(objectType, Content, jsonOptions),
				EventContentType.None => null,
				_ => throw new NotSupportedException($"The content type '{ContentType}' is not supported for conversion to an object")
			};
		}

		private object? ConvertToObject(Type objectType, object content, JsonSerializerOptions? jsonOptions) {
			if (objectType.IsInstanceOfType(content) || content == null)
				return content;

			if (content is byte[] bytes)
				return JsonSerializer.Deserialize(bytes, objectType, jsonOptions);
			if (content is string str)
				return JsonSerializer.Deserialize(str, objectType, jsonOptions);

			var json = JsonSerializer.Serialize(content, content.GetType(), jsonOptions);
			return JsonSerializer.Deserialize(json, objectType, jsonOptions);
		}

		public static bool operator ==(EventData left, EventData right) {
			return left.isEmpty == right.isEmpty && Equals(left.Content, right.Content);
		}

		public static bool operator !=(EventData left, EventData right) {
			return !(left == right);
		}
	}
}
