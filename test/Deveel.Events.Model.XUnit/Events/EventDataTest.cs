using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Deveel.Events {
	public static class EventDataTest {
		[Fact]
		public static void CreateEventData() {
			var eventData = new EventData(new byte[] { 0x01, 0x02, 0x03 });

			Assert.NotEqual(EventData.Empty, eventData);
			Assert.Equal(new byte[] { 0x01, 0x02, 0x03 }, eventData.Content);
		}

		[Fact]
		public static void CreateEmptyEventData() {
			var eventData = EventData.Empty;

			Assert.Equal(EventData.Empty, eventData);
			Assert.Null(eventData.Content);
		}

		[Fact]
		public static void CreateEventDataFromSpan() {
			var eventData = new EventData(new ReadOnlySpan<byte>(new byte[] { 0x01, 0x02, 0x03 }));

			Assert.NotEqual(EventData.Empty, eventData);
			Assert.Equal(new byte[] { 0x01, 0x02, 0x03 }, eventData.Content);
		}

		[Fact]
		public static void CreateEventDataFromNull() {
			var eventData = new EventData(null);

			Assert.NotEqual(EventData.Empty, eventData);
			Assert.Null(eventData.Content);
		}

		[Fact]
		public static void CreateEventDataFromObject() {
			var eventData = new EventData(new {
				first_name = "John",
				last_name = "Doe"
			});

			Assert.NotEqual(EventData.Empty, eventData);
			Assert.NotNull(eventData.Content);
			Assert.Equal(EventContentType.Object, eventData.ContentType);

			var json = JsonSerializer.Serialize(new {
				first_name = "John",
				last_name = "Doe"
			});

			Assert.Equal(json, JsonSerializer.Serialize(eventData.Content));
		}

		[Fact]
		public static void CreateEventDataFromObjectWithJsonOptions() {
			var options = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			var eventData = new EventData(new {
				first_name = "John",
				last_name = "Doe"
			}, options);

			Assert.NotEqual(EventData.Empty, eventData);
			Assert.NotNull(eventData.Content);
			Assert.Equal(EventContentType.Json, eventData.ContentType);

			var json = JsonSerializer.SerializeToUtf8Bytes(new {
				first_name = "John",
				last_name = "Doe"
			}, options);

			Assert.Equal(json, eventData.Content);
		}

		[Fact]
		public static void CreateEventDataFromJsonString() {
			var eventData = new EventData("{\"first_name\":\"John\",\"last_name\":\"Doe\"}", EventContentType.Json);

			Assert.NotEqual(EventData.Empty, eventData);
			Assert.NotNull(eventData.Content);
			Assert.Equal(EventContentType.Json, eventData.ContentType);

			Assert.Equal("{\"first_name\":\"John\",\"last_name\":\"Doe\"}", Encoding.UTF8.GetString((byte[]) eventData.Content));
		}

		[Fact]
		public static void CreateJsonEventDataFromEmptyString() {
			var eventData = new EventData(String.Empty, EventContentType.Json);

			Assert.NotEqual(EventData.Empty, eventData);
			Assert.Equal(Array.Empty<byte>(), eventData.Content);
			Assert.Equal(EventContentType.Json, eventData.ContentType);
		}

		[Fact]
		public static void CreateJsonEventDataFromNull() {
			var eventData = new EventData((object?) null, EventContentType.Json);

			Assert.NotEqual(EventData.Empty, eventData);
			Assert.Null(eventData.Content);
			Assert.Equal(EventContentType.Json, eventData.ContentType);

			Assert.Null(eventData.AsJsonNode());
		}

		[Fact]
		public static void ObjectDataAsJsonNode() {
			var eventData = new EventData(new {
				first_name = "John",
				last_name = "Doe"
			});

			var jsonNode = eventData.AsJsonNode();

			Assert.NotNull(jsonNode);
			Assert.Equal("John", jsonNode["first_name"].GetValue<string>());
			Assert.Equal("Doe", jsonNode["last_name"].GetValue<string>());
		}

		[Fact]
		public static void ObjectDataAsJson() {
			var eventData = new EventData(new {
				first_name = "John",
				last_name = "Doe"
			});

			var json = eventData.AsJson();

			Assert.NotNull(json);
			Assert.Equal("{\"first_name\":\"John\",\"last_name\":\"Doe\"}", json);
		}

		[Fact]
		public static void StringDataAsJsonNode() {
			var eventData = new EventData("{\"first_name\":\"John\",\"last_name\":\"Doe\"}", EventContentType.Json);

			var jsonNode = eventData.AsJsonNode();

			Assert.NotNull(jsonNode);
			Assert.Equal("John", jsonNode["first_name"].GetValue<string>());
			Assert.Equal("Doe", jsonNode["last_name"].GetValue<string>());
		}

		[Fact]
		public static void StringDataAsJson() {
			var eventData = new EventData("{\"first_name\":\"John\",\"last_name\":\"Doe\"}", EventContentType.Json);

			var json = eventData.AsJson();

			Assert.NotNull(json);
			Assert.Equal("{\"first_name\":\"John\",\"last_name\":\"Doe\"}", json);
		}

		[Fact]
		public static void BinaryDataAsJsonNode() {
			var eventData = new EventData(JsonSerializer.Serialize(new { 
				first_name = "John",
				last_name = "Doe"
			}), EventContentType.Binary);

			var jsonNode = eventData.AsJsonNode();

			Assert.NotNull(jsonNode);
			Assert.Equal("John", jsonNode["first_name"].GetValue<string>());
			Assert.Equal("Doe", jsonNode["last_name"].GetValue<string>());
		}

		[Fact]
		public static void BinaryDataAsJson() {
			var eventData = new EventData(JsonSerializer.Serialize(new {
				first_name = "John",
				last_name = "Doe"
			}), EventContentType.Binary);

			var json = eventData.AsJson();

			Assert.NotNull(json);
			Assert.Equal("{\"first_name\":\"John\",\"last_name\":\"Doe\"}", json);
		}

		[Fact]
		public static void EmptyDataAsJsonNode() {
			var eventData = EventData.Empty;

			var jsonNode = eventData.AsJsonNode();

			Assert.Null(jsonNode);
		}

		[Fact]
		public static void EmptyDataAsJson() {
			var eventData = EventData.Empty;

			var json = eventData.AsJson();

			Assert.Null(json);
		}

		[Fact]
		public static void ObjectDataAsJsonNodeWithJsonOptions() {
			var options = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			var eventData = new EventData(new {
				first_name = "John",
				last_name = "Doe"
			}, options);

			var jsonNode = eventData.AsJsonNode(options);

			Assert.NotNull(jsonNode);
			Assert.Equal("John", jsonNode["first_name"].GetValue<string>());
			Assert.Equal("Doe", jsonNode["last_name"].GetValue<string>());
		}

		[Fact]
		public static void ObjectDataAsJsonWithJsonOptions() {
			var options = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			var eventData = new EventData(new {
				first_name = "John",
				last_name = "Doe"
			}, options);

			var json = eventData.AsJson(options);

			Assert.NotNull(json);
			Assert.Equal("{\"first_name\":\"John\",\"last_name\":\"Doe\"}", json);
		}

		[Fact]
		public static void StringDataAsJsonNodeWithJsonOptions() {
			var options = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			var eventData = new EventData("{\"first_name\":\"John\",\"last_name\":\"Doe\"}", EventContentType.Json);

			var jsonNode = eventData.AsJsonNode(options);

			Assert.NotNull(jsonNode);
			Assert.Equal("John", jsonNode["first_name"].GetValue<string>());
			Assert.Equal("Doe", jsonNode["last_name"].GetValue<string>());
		}

		[Fact]
		public static void StringDataAsJsonWithJsonOptions() {
			var options = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			var eventData = new EventData("{\"first_name\":\"John\",\"last_name\":\"Doe\"}", EventContentType.Json);

			var json = eventData.AsJson(options);

			Assert.NotNull(json);
			Assert.Equal("{\"first_name\":\"John\",\"last_name\":\"Doe\"}", json);
		}

		[Fact]
		public static void ObjectDataAsObject() {
			var eventData = new EventData(new {
				first_name = "John",
				last_name = "Doe"
			});

			var person = eventData.AsObject<Person>();

			Assert.NotNull(person);

			Assert.Equal("John", person.FirstName);
			Assert.Equal("Doe", person.LastName);
		}

		[Fact]
		public static void StringDataAsObject() {
			var eventData = new EventData("{\"first_name\":\"John\",\"last_name\":\"Doe\"}", EventContentType.Json);

			var person = eventData.AsObject<Person>();

			Assert.NotNull(person);

			Assert.Equal("John", person.FirstName);
			Assert.Equal("Doe", person.LastName);
		}

		[Fact]
		public static void JsonDataAsObject() {
			var eventData = new EventData(JsonSerializer.Serialize(new {
				first_name = "John",
				last_name = "Doe"
			}), EventContentType.Json);

			Assert.Equal(EventContentType.Json, eventData.ContentType);
			Assert.IsType<byte[]>(eventData.Content);

			var person = eventData.AsObject<Person>();

			Assert.NotNull(person);

			Assert.Equal("John", person.FirstName);
			Assert.Equal("Doe", person.LastName);
		}

		[Fact]
		public static void EmptyDataAsObject() {
			var eventData = EventData.Empty;

			var person = eventData.AsObject<Person>();

			Assert.Null(person);
		}

		[Fact]
		public static void EmptyDataAsObjectAsType() {
			var eventData = EventData.Empty;

			var person = eventData.AsObject(typeof(Person));

			Assert.Null(person);
		}

		[Fact]
		public static void NullDataAsObject() {
			var eventData = new EventData((object?) null);

			var person = eventData.AsObject<Person>();

			Assert.Null(person);
		}

		class Person {
			[JsonPropertyName("first_name")]
			public string FirstName { get; set; }

			[JsonPropertyName("last_name")]
			public string LastName { get; set; }
		}
	}
}
