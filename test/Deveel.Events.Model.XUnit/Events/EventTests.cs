using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Deveel.Events {
	public static class EventTests {
		[Fact]
		public static void CreateEvent() {
			var @event = new Event("test", "1.0");

			Assert.Equal("test", @event.EventType);
			Assert.Equal("1.0", @event.DataVersion);
			Assert.Null(@event.EventId);
			Assert.Null(@event.Source);
			Assert.Equal(EventData.Empty, @event.EventData);
		}

		[Fact]
		public static void CreateEventWithId() {
			var @event = new Event("test", "1.0")
				.WithEventId("12345");

			Assert.Equal("test", @event.EventType);
			Assert.Equal("1.0", @event.DataVersion);
			Assert.Equal("12345", @event.EventId);
			Assert.Null(@event.Source);
			Assert.Equal(EventData.Empty, @event.EventData);
		}

		[Fact]
		public static void CreateEventWithSource() {
			var @event = new Event("test", "1.0")
				.WithSource("test-source");

			Assert.Equal("test", @event.EventType);
			Assert.Equal("1.0", @event.DataVersion);
			Assert.Null(@event.EventId);
			Assert.Equal("test-source", @event.Source);
			Assert.Equal(EventData.Empty, @event.EventData);
		}

		[Fact]
		public static void CreateEventWithTimeStamp() {
			var timeStamp = new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero);
			var @event = new Event("test", "1.0")
				.WithTimeStamp(timeStamp);

			Assert.Equal("test", @event.EventType);
			Assert.Equal("1.0", @event.DataVersion);
			Assert.Null(@event.EventId);
			Assert.Null(@event.Source);
			Assert.Equal(EventData.Empty, @event.EventData); 
			Assert.Equal(timeStamp, @event.TimeStamp);
		}

		[Fact]
		public static void CreateEventWithProperties() {
			var @event = new Event("test", "1.0")
				.With(new Dictionary<string, object?> {
					{"key1", "value1"},
					{"key2", 12345}
				});

			Assert.Equal("test", @event.EventType);
			Assert.Equal("1.0", @event.DataVersion);
			Assert.Null(@event.EventId);
			Assert.Null(@event.Source);
			Assert.Equal(EventData.Empty, @event.EventData); 
			Assert.Equal("value1", @event.Attributes["key1"]);
			Assert.Equal(12345, @event.Attributes["key2"]);
		}

		[Fact]
		public static void CreateEventWithOverriddenProperties() {
			var @event = new Event("test", "1.0");
			@event.Attributes["key1"] = "value1";
			@event.Attributes["key2"] = 12345;

			var otherEvent = @event
				.With(new Dictionary<string, object?> {
					{"key1", "value2"},
					{"key3", 67890}
				});

			Assert.Equal("test", otherEvent.EventType);
			Assert.Equal("1.0", otherEvent.DataVersion);
			Assert.Null(otherEvent.EventId);
			Assert.Null(otherEvent.Source);
			Assert.Equal(EventData.Empty, @event.EventData); 
			Assert.Equal("value2", otherEvent.Attributes["key1"]);
			Assert.Equal(12345, otherEvent.Attributes["key2"]);
			Assert.Equal(67890, otherEvent.Attributes["key3"]);
		}

		[Fact]
		public static void CreateEventWithStringBinaryData() {
			const string eventData = "eventContent";
			var @event = new Event("test", "1.0")
				.WithBinaryData(eventData);

			var binaryData = Encoding.UTF8.GetBytes(eventData);

			Assert.Equal("test", @event.EventType);
			Assert.Equal("1.0", @event.DataVersion);
			Assert.Null(@event.EventId);
			Assert.Null(@event.Source);
			var bytes = Assert.IsType<byte[]>(@event.EventData.Content);
			Assert.Equal(binaryData, bytes);
		}

		[Fact]
		public static void CreateEventWithJsonData() {
			var person = new PersonCreated {
				FirstName = "John",
				LastName = "Doe",
				Id = "12345",
				Age = 30
			};

			var @event = new Event("person.created", "1.0")
				.WithJsonData(person);

			var json = JsonSerializer.Serialize(person);
			var binaryData = Encoding.UTF8.GetBytes(json);

			Assert.Equal("person.created", @event.EventType);
			Assert.Equal("1.0", @event.DataVersion);
			Assert.Null(@event.EventId);
			Assert.Null(@event.Source);
			var bytes = Assert.IsType<byte[]>(@event.EventData.Content);
			Assert.Equal(binaryData, bytes);
		}

		[Fact]
		public static void CreateFromData() {
			var @event = Event.FromData(new PersonCreated {
				FirstName = "John",
				LastName = "Doe",
				Id = "12345",
				Age = 30
			});

			Assert.Equal("person.created", @event.EventType);
			Assert.Equal("1.0", @event.DataVersion);
			Assert.Null(@event.EventId);
			Assert.Null(@event.Source);
			Assert.NotNull(@event.Attributes);
			Assert.NotEmpty(@event.Attributes);
			Assert.Contains("stream-type", @event.Attributes.Keys);
			Assert.Equal("person", @event.Attributes["stream-type"]);
			Assert.Equal(EventContentType.Object, @event.EventData.ContentType);

			var personCreated = Assert.IsType<PersonCreated>(@event.EventData.Content);

			Assert.Equal("John", personCreated.FirstName);
			Assert.Equal("Doe", personCreated.LastName);
			Assert.Equal("12345", personCreated.Id);
			Assert.Equal(30, personCreated.Age);
		}

		[Fact]
		public static void CreateFromDataWithoutEventAttribute() {
			Assert.Throws<ArgumentException>(() => Event.FromData(new {Name = "John"}));
		}

		[Event("person.created", "1.0")]
		[EventAttributes("stream-type", "person")]
		class PersonCreated {
			[JsonPropertyName("first_name"), Required]
			public string FirstName { get; set; }

			[JsonPropertyName("last_name"), Required]
			public string LastName { get; set; }

			[JsonPropertyName("id"), Required]
			public string Id { get; set; }

			[JsonPropertyName("age")]
			public int? Age { get; set; }

			[JsonPropertyName("email")]
			public Email? Email { get; set; }
		}

		class Email {
			[JsonPropertyName("display_name")]
			public string? DisplayName { get; set; }

			[JsonPropertyName("address"), Required]
			public string Address { get; set; }

			[JsonPropertyName("type")]
			public string? Type { get; set; }
		}
	}
}